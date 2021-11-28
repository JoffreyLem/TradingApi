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

    public class StrategyService : IStrategyService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<IdentityUser<int>> _userManager;

        private readonly IApiHandler _apiHandler;
        private readonly ApiTradingDatabaseContext _context;

        public StrategyService(IApiHandler apiHandler, ApiTradingDatabaseContext apiDbContext,
            RoleManager<IdentityRole<int>> roleManager, UserManager<IdentityUser<int>> userManager)
        {
            _apiHandler = apiHandler;
            _context = apiDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
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
            await _context.SignalInfoStrategies.AddAsync(signalInfoStrategy);

            return new BaseResponse("Signal ajouter");
        }

        public async Task<BaseResponse<List<string>>> GetUsersGiverSignal()
        {
            var systemUser = await _userManager.FindByNameAsync("System");
            var data = _context.SignalInfoStrategies
                .Where(x => x.User != systemUser)
                .GroupBy(x=>x.User.UserName)
                .Select(x=>x.First()).Select(x=>x.User.UserName).ToList();

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
            var dataSignal = _context.SignalInfoStrategies.Where(x =>
                x.Symbol == symbol && x.Timeframe == timeframe && x.Strategy == strategy.Description).ToList();

            var lastSignal = dataSignal.LastOrDefault();
            int? index = null;

            if (lastSignal != null)
                index = strategy.History.Where(x => x.Date > lastSignal.DateTime).Select((candle, i) => i).First();

            var dataSignalsAnalyzed = await strategy.Run(index);
            await SaveSignal(dataSignalsAnalyzed);
            dataSignal.AddRange(dataSignalsAnalyzed);

            return dataSignal.OrderByDescending(x => x.DateTime).ToList();
        }

        private async Task<List<SignalInfoStrategy>> GetSignalOfUser(string symbol, string timeframe, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);


            var dataSignal = _context.SignalInfoStrategies.Where(x => x.User == user).Where(x => x.Symbol == symbol)
                .Where(x => x.Timeframe == timeframe).ToList();


            return dataSignal.OrderByDescending(x => x.DateTime).ToList();
        }

        private async Task SaveSignal(List<SignalInfoStrategy> signals, IdentityUser<int> user = null)
        {
            if (user is null) user = await _userManager.FindByNameAsync("System");

            Parallel.ForEach(signals, strategy => { strategy.User = user; });


            await _context.SignalInfoStrategies.AddRangeAsync(signals);
            await _context.SaveChangesAsync();
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