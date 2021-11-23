using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApiTrading.Exception;
using ApiTrading.Modele;
using ApiTrading.Modele.DTO.Response;
using ApiTrading.Service.ExternalAPIHandler;
using Modele;
using Strategy;
using StrategyManager;
using StrategyList = ApiTrading.Modele.DTO.Response.StrategyList;

namespace ApiTrading.Service.Strategy
{
    public class StrategyService : IStrategyService
    {
      
        private  IApiHandler _apiHandler;
        public StrategyService(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler;

        }
        
        public async Task<StrategyResponse> GetAllStrategy()
        {
            var strategyResponses = new StrategyResponse();
            foreach (Enum enumVal in Enum.GetValues(typeof(StrategyManager.StrategyList)))
            {
                var strategyList = new StrategyList();
                
                var memInfo = enumVal?.GetType().GetMember(enumVal.ToString());
                var attribute = (memInfo?[0] ?? throw new InvalidOperationException())
                    .GetCustomAttribute<StrategyAttributeType>();
                strategyList.Name = attribute?.Name;
                strategyList.Description = attribute?.Description;
                strategyResponses.StrategyLists.Add(strategyList);
            }
            return strategyResponses;
        }

        public async Task<TimeframeResponse> GetAllTimeframe()
        {
            var timeframersp = new TimeframeResponse();
            var listTf = new List<string>();
            foreach (Enum enumVal in Enum.GetValues(typeof(Timeframe)))
            {
                var memInfo = enumVal?.GetType().GetMember(enumVal.ToString());
                var attribute = (memInfo?[0] ?? throw new InvalidOperationException())
                    .GetCustomAttribute<DescriptionAttribute>();
                listTf.Add(attribute?.Description);
            }

            timeframersp.Timeframes = listTf;

            return timeframersp;
        }

        public async Task<SignalResponse> GetSignals(string strategy, string symbol, string timeframe)
        {
            var strategyInitialized = GetStrategyType(strategy);
            var data = await _apiHandler.GetAllChart(symbol, timeframe,null);
            var data2 = data.Data;
            strategyInitialized.History = data2;
            var dataSignals =await strategyInitialized.Run();
            var signalResponse = new SignalResponse(dataSignals.Select(x=>new ApiTrading.Modele.SignalInfo(x)).ToList());

            return signalResponse;
        }

        private global::Strategy.Strategy GetStrategyType(string strategy)
        {
            foreach (Enum enumVal in Enum.GetValues(typeof(Timeframe)))
            {
                var memInfo = enumVal?.GetType().GetMember(enumVal.ToString());
                var attribute = (memInfo?[0] ?? throw new InvalidOperationException())
                    .GetCustomAttribute<StrategyAttributeType>();

                if (attribute?.Name == strategy)
                {
                    var type = attribute?.Type;
                    
                    return StrategyFactory.GetStrategy(type);

                }
            }

            throw new NotFoundException("La strategy n'existe pas");

        }
    }
}