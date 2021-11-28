namespace ApiTrading.Service.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using DbContext;
    using Exception;
    using ExternalAPIHandler;
    using global::Modele;
    using global::Strategy;
    using global::StrategyManager;
    using Microsoft.AspNetCore.Identity;
    using Modele.DTO.Request;
    using Modele.DTO.Response;
    using Repository.Signal;
    using Repository.Utilisateurs;

    public class StrategyService : IStrategyService
    {
       

        private readonly IApiHandler _apiHandler;
        private readonly IUserRepository _userRepository;
        private readonly ISignalRepository _signalRepository;

        public StrategyService(IApiHandler apiHandler, IUserRepository userRepository, ISignalRepository signalRepository)
        {
            _apiHandler = apiHandler;
            _userRepository = userRepository;
            _signalRepository = signalRepository;
        }

        

        public async Task<BaseResponse<List<StrategyList>>> GetAllStrategy()
        {
            var rsp = new BaseResponse<List<StrategyList>>();

            foreach (Enum enumVal in Enum.GetValues(typeof(StrategyManager.StrategyList)))
            {
                var strategyList = new StrategyList();

                var memInfo = enumVal?.GetType().GetMember(enumVal.ToString());
                var attribute = (memInfo?[0] ?? throw new InvalidOperationException())
                    .GetCustomAttribute<StrategyAttributeType>();
                strategyList.Name = attribute?.Name;
                strategyList.Description = attribute?.Description;
                rsp.Data.Add(strategyList);
            }

            return rsp;
        }

        public async Task<BaseResponse<List<string>>> GetAllTimeframe()
        {
            var timeframersp = new BaseResponse<List<string>>();
            var listTf = new List<string>();
            foreach (Enum enumVal in Enum.GetValues(typeof(Timeframe)))
            {
                var memInfo = enumVal?.GetType().GetMember(enumVal.ToString());
                var attribute = (memInfo?[0] ?? throw new InvalidOperationException())
                    .GetCustomAttribute<DescriptionAttribute>();
                listTf.Add(attribute?.Description);
            }

            timeframersp.Data = listTf;

            return timeframersp;
        }

        public async Task<BaseResponse<List<SignalInfoStrategy>>> GetSignals(string strategy, string symbol,
            string timeframe, string user)
        {
            var signalResponse = new BaseResponse<List<SignalInfoStrategy>>();

            var dataChart = await _apiHandler.GetAllChart(symbol, timeframe);
            var data2 = dataChart.Data;
            var strategyInitialized = GetStrategyType(strategy, symbol, timeframe);
            strategyInitialized.History = data2;
            if (user == null)
            {
                if (string.IsNullOrEmpty(strategy))
                {
                    throw new StrategyNotGivenException("Le nom de la strategy est nécecssaire");
                }
                var dataSystem = await GetSignalOfSystem(strategyInitialized, symbol, timeframe);
                signalResponse.Data = dataSystem;
            }
            else
            {
                var dataUser = await GetSignalOfUser(symbol, timeframe, user);
                signalResponse.Data = dataUser;
            }

            if (signalResponse.Data.Count == 0) signalResponse.Message = "Aucuns signals disponible sur la période";

            return signalResponse;
        }

        public async Task<BaseResponse> PostSignal(SignalInfoRequest infoRequest, IdentityUser<int> user)
        {
            if (!_apiHandler.AllSymbolList.Any(x => x.Symbol == infoRequest.Symbol))
                throw new NotFoundException("Le symbol n'existe pas");

            var signalInfoStrategy = new SignalInfoStrategy();
            signalInfoStrategy.Symbol = infoRequest.Symbol;
            signalInfoStrategy.Timeframe = infoRequest.Timeframe;
            signalInfoStrategy.EntryLevel = infoRequest.EntryLevel;
            signalInfoStrategy.StopLoss = infoRequest.StopLoss;
            signalInfoStrategy.TakeProfit = infoRequest.TakeProfit;
            signalInfoStrategy.User = user;
            signalInfoStrategy.Strategy = "";
            signalInfoStrategy.Signal = infoRequest.Signal;
            signalInfoStrategy.DateTime = infoRequest.DateTime;
            await _signalRepository.SaveSignal(signalInfoStrategy);

            return new BaseResponse("Signal ajouter");
        }

        public async Task<BaseResponse<List<string>>> GetUsersGiverSignal()
        {
         
            var data = await _signalRepository.GetAllGiverSignal();

            return new BaseResponse<List<string>>(data);
        }

        public async Task<BaseResponse> SubscribeToSymbolInfo(string symbol)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> UnsubscribeToSymbolInfo(string symbol)
        {
            throw new NotImplementedException();
        }

        private async Task<List<SignalInfoStrategy>> GetSignalOfSystem(Strategy strategy, string symbol,
            string timeframe)
        {
            var dataSignal =await _signalRepository.GetSignalsOfSystem(strategy.Description, symbol, timeframe);

            var lastSignal = dataSignal.LastOrDefault();
            int? index = null;

            if (lastSignal != null)
                index = strategy.History.Where(x => x.Date > lastSignal.DateTime).Select((candle, i) => i).First();

            var dataSignalsAnalyzed = await strategy.Run(index);
            await  _signalRepository.SaveSignals(dataSignalsAnalyzed);
            dataSignal.AddRange(dataSignalsAnalyzed);
            return dataSignal.OrderByDescending(x => x.DateTime).ToList();
        }

        private async Task<List<SignalInfoStrategy>> GetSignalOfUser(string symbol, string timeframe, string userName)
        {
            var user = await _userRepository.FindByNameAsync(userName);
            var dataSignal = await _signalRepository.GetSignalsOfUser(symbol, timeframe, userName);
            return dataSignal.OrderByDescending(x => x.DateTime).ToList();
        }

        

        private Strategy GetStrategyType(string strategy, string symbol, string timeframe)
        {
            foreach (Enum enumVal in Enum.GetValues(typeof(StrategyManager.StrategyList)))
            {
                var memInfo = enumVal?.GetType().GetMember(enumVal.ToString());
                var attribute = (memInfo?[0] ?? throw new InvalidOperationException())
                    .GetCustomAttribute<StrategyAttributeType>();

                if (attribute?.Name == strategy)
                {
                    var type = attribute?.Type;

                    return StrategyFactory.GetStrategy(type, symbol, timeframe);
                }
            }

            throw new NotFoundException("La strategy n'existe pas");
        }
    }
}