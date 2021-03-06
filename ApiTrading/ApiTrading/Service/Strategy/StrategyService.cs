using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ApiTrading.Exception;
using ApiTrading.Filter;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using ApiTrading.Repository.Signal;
using ApiTrading.Repository.Utilisateurs;
using ApiTrading.Service.ExternalAPIHandler;
using ApiTrading.Service.Mail;
using Microsoft.AspNetCore.Http;
using Modele;
using Strategy;
using StrategyManager;
using Utility;

namespace ApiTrading.Service.Strategy
{
    public class StrategyService : IStrategyService
    {
        private readonly IApiHandler _apiHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMail _mailService;
        private readonly ISignalRepository _signalRepository;
        private readonly IUserRepository _userRepository;

        public StrategyService(IApiHandler apiHandler, IUserRepository userRepository,
            ISignalRepository signalRepository, IHttpContextAccessor httpContextAccessor, IMail mailService)
        {
            _apiHandler = apiHandler;
            _userRepository = userRepository;
            _signalRepository = signalRepository;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
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
            CheckTimeFrame(timeframe);

            var signalResponse = new BaseResponse<List<SignalInfoStrategy>>();


            if (user == null)
            {
                var dataChart = await _apiHandler.GetAllChart(symbol, timeframe);
                var data2 = dataChart.Data;
                var strategyInitialized = GetStrategyType(strategy, symbol, timeframe);
                strategyInitialized.History = data2;
                if (string.IsNullOrEmpty(strategy))
                    throw new StrategyNotGivenException("Le nom de la strategy est n??cecssaire");
                var dataSystem = await GetSignalOfSystem(strategyInitialized, symbol, timeframe);
                signalResponse.Data = dataSystem;
            }
            else
            {
                var dataUser = await GetSignalOfUser(symbol, timeframe, user);
                signalResponse.Data = dataUser;
            }

            if (signalResponse.Data.Count == 0) signalResponse.Message = "Aucuns signals disponible sur la p??riode";

            return signalResponse;
        }

        public async Task<BaseResponse> PostSignal(SignalInfoRequest infoRequest)
        {
            if (!_apiHandler.AllSymbolList.Any(x => x.Symbol == infoRequest.Symbol))
                throw new NotFoundException("Le symbol n'existe pas");
            var user = _httpContextAccessor.HttpContext.GetCurrentUser();
            var signalInfoStrategy = new SignalInfoStrategy();
            signalInfoStrategy.Symbol = infoRequest.Symbol;
            signalInfoStrategy.Timeframe = infoRequest.Timeframe;
            signalInfoStrategy.EntryLevel = infoRequest.EntryLevel;
            signalInfoStrategy.StopLoss = infoRequest.StopLoss;
            signalInfoStrategy.TakeProfit = infoRequest.TakeProfit;
            signalInfoStrategy.User = user;
            signalInfoStrategy.Strategy = "";
            signalInfoStrategy.Signal = infoRequest.Signal.GetEnumDescription();
            signalInfoStrategy.DateTime = infoRequest.DateTime;
            await _signalRepository.SaveSignal(signalInfoStrategy);
            await SendMailSubscription(infoRequest.Symbol, user.UserName);
            return new BaseResponse("Signal ajouter");
        }

        public async Task<BaseResponse<List<string>>> GetUsersGiverSignal()
        {
            var data = await _signalRepository.GetAllGiverSignal();

            return new BaseResponse<List<string>>(data);
        }

        public async Task<BaseResponse> SubscribeToSymbolInfo(string modelUser, string symbol)
        {
            var user = await _userRepository.FindByNameAsync(modelUser);
            if (user is null) throw new NotFoundException("L'utilisateur n'existe pas");

            if (!_apiHandler.AllSymbolList.Any(x => x.Symbol == symbol))
                throw new NotFoundException("Le symbole n'existe pas");
            await _signalRepository.SubscribeToSignal(user, symbol);
            return new BaseResponse("Subscription ok");
        }

        public async Task<BaseResponse> UnsubscribeToSymbolInfo(string symbol)
        {
            var user = _httpContextAccessor.HttpContext.GetCurrentUser();
            await _signalRepository.UnsubscribeToSignal(user, symbol);
            return new BaseResponse("UnSubscription ok");
        }


        public async Task<BaseResponse<List<SubscriptionResponse>>> GetCurrentSignalSubscription()
        {
            var user = _httpContextAccessor.HttpContext.GetCurrentUser();
            var data = await _signalRepository.GetCurrentSignalSubscription(user);
            var rsp = data.Select(x => new SubscriptionResponse(x)).ToList();
            return new BaseResponse<List<SubscriptionResponse>>(rsp);
        }

        private async Task<List<SignalInfoStrategy>> GetSignalOfSystem(global::Strategy.Strategy strategy,
            string symbol,
            string timeframe)
        {
            CheckTimeFrame(timeframe);
            var dataSignal = await _signalRepository.GetSignalsOfSystem(strategy.Description, symbol, timeframe);

            var lastSignal = dataSignal.LastOrDefault();
            int? index = null;

            if (lastSignal != null)
                index = strategy.History.Where(x => x.Date > lastSignal.DateTime).Select((candle, i) => i).First();

            var dataSignalsAnalyzed = await strategy.Run(index);
            await _signalRepository.SaveSignals(dataSignalsAnalyzed);
            dataSignal.AddRange(dataSignalsAnalyzed);
            return dataSignal.OrderByDescending(x => x.DateTime).ToList();
        }

        private async Task SendMailSubscription(string symbol, string sender)
        {
            var subscription = await _signalRepository.GetSubscriptionsOfSymbol(symbol);
            var message = new StringBuilder();
            message.Append("Nouveau message disponible !\n");

            foreach (var sub in subscription)
            {
                message.Append($"Un nouveau signal sur {symbol} de {sender}");
                await _mailService.Send(sub.User.Email, $"[{symbol}] Nouveau signal disponible", message.ToString());
            }
        }

        private async Task<List<SignalInfoStrategy>> GetSignalOfUser(string symbol, string timeframe, string userName)
        {
            CheckTimeFrame(timeframe);
            var user = await _userRepository.FindByNameAsync(userName);
            if (user is null) throw new NotFoundException("L'utilisateur n'existe pas");
            var dataSignal = await _signalRepository.GetSignalsOfUser(symbol, timeframe, user);
            return dataSignal.OrderByDescending(x => x.DateTime).ToList();
        }


        private global::Strategy.Strategy GetStrategyType(string strategy, string symbol, string timeframe)
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


        private bool CheckTimeFrame(string periodCode)
        {
            switch (periodCode)
            {
                case "1m":

                    return true;

                case "5m":
                    return true;

                case "15m":
                    return true;

                case "30m":
                    return true;

                case "1h":
                    return true;

                case "4h":
                    return true;

                case "1d":
                    return true;

                case "1w":
                    return true;


                case "1mn":
                    return true;


                default:
                    var message = "le timeframe n'existe pas";
                    throw new TimeFrameDontExistException(message);
            }
        }
    }
}