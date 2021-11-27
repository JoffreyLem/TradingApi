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
    using DbContext;
    using Microsoft.AspNetCore.Identity;
    using Strategy = global::Strategy.Strategy;

    public class StrategyService : IStrategyService
    {
      
        private  IApiHandler _apiHandler;
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private ApiTradingDatabaseContext _context;
        public StrategyService(IApiHandler apiHandler, ApiTradingDatabaseContext apiDbContext, RoleManager<IdentityRole<int>> roleManager, UserManager<IdentityUser<int>> userManager)
        {
            _apiHandler = apiHandler;
            _context = apiDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
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

        public async Task<SignalResponse> GetSignals(string strategy, string symbol, string timeframe,IdentityUser<int> user = null)
        {
            
            
       
            var data = await _apiHandler.GetAllChart(symbol, timeframe,true);
            var data2 = data.Data;
            var strategyInitialized = GetStrategyType(strategy,symbol,timeframe,data2);
            strategyInitialized.History = data2;
            var dataSignals =await strategyInitialized.Run();
            await SaveSignal(dataSignals,user);
            var signalResponse = new SignalResponse(dataSignals.Select(x=>new ApiTrading.Modele.SignalInfo(x)).ToList());

            return signalResponse;
        }

        private async Task SaveSignal(List<SignalInfoStrategy> signals, IdentityUser<int> user = null)
        {
            if (user is null)
            {
                user = await _userManager.FindByIdAsync("1");
            }

            foreach (var signalInfoStrategy in signals)
            {
                signalInfoStrategy.User = user;

               await _context.SignalInfoStrategies.AddAsync(signalInfoStrategy);
               await _context.SaveChangesAsync();
            }
        }

        private Strategy GetStrategyType(string strategy, string symbol, string timeframe, List<Candle> data)
        {
            foreach (Enum enumVal in Enum.GetValues(typeof(StrategyManager.StrategyList)))
            {
                var memInfo = enumVal?.GetType().GetMember(enumVal.ToString());
                var attribute = (memInfo?[0] ?? throw new InvalidOperationException())
                    .GetCustomAttribute<StrategyAttributeType>();

                if (attribute?.Name == strategy)
                {
                    var type = attribute?.Type;
                    
                    return StrategyFactory.GetStrategy(type,symbol,timeframe,data);

                }
            }

            throw new NotFoundException("La strategy n'existe pas");

        }
    }
}