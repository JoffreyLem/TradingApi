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
    using Controllers;
    using DbContext;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.OpenApi.Any;
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

        public async Task<BaseResponse<List<SignalInfoStrategy>>> GetSignals(string strategy, string symbol, string timeframe,string user)
        {
            var signalResponse = new BaseResponse<List<SignalInfoStrategy>>();
            
            var dataChart = await _apiHandler.GetAllChart(symbol, timeframe,true);
            var data2 = dataChart.Data;
            var strategyInitialized = GetStrategyType(strategy,symbol,timeframe);
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
            if (signalResponse.Data.Count == 0)
            {
                 signalResponse.Message = $"Aucuns signals disponible sur la période";
            }
                    
            return signalResponse;
            
        }

        private async Task<List<SignalInfoStrategy>> GetSignalOfSystem(Strategy strategy, string symbol, string timeframe)
        {
            
            List<SignalInfoStrategy> dataSignal = _context.SignalInfoStrategies.Where(x=>x.Symbol == symbol && x.Timeframe== timeframe && x.Strategy==strategy.Description).ToList();

            var lastSignal = dataSignal.LastOrDefault();

            if (lastSignal != null)
            {
                strategy.History = strategy.History.Where(x => x.Date > lastSignal.DateTime).ToList();
            }

            var dataSignalsAnalyzed =await strategy.Run();
            await SaveSignal(dataSignalsAnalyzed);
            dataSignal.ToList().AddRange(dataSignalsAnalyzed);
            
            return dataSignal.OrderByDescending(x => x.DateTime).ToList();

        }

        private async Task<List<SignalInfoStrategy>> GetSignalOfUser( string symbol, string timeframe, string userName)
        {
            var user =await _userManager.FindByNameAsync(userName);
            
         
            List<SignalInfoStrategy> dataSignal = _context.SignalInfoStrategies.Where(x => x.User == user).Where(x=>x.Symbol== symbol).Where(x=>x.Timeframe==timeframe).ToList();
            

            return dataSignal.OrderByDescending(x=>x.DateTime).ToList();
        }

        private async Task SaveSignal(List<SignalInfoStrategy> signals, IdentityUser<int> user = null)
        {
            if (user is null)
            {
                user = await _userManager.FindByNameAsync("System");
            }

            foreach (var signalInfoStrategy in signals)
            {
                signalInfoStrategy.User = user;

               await _context.SignalInfoStrategies.AddAsync(signalInfoStrategy);
               await _context.SaveChangesAsync();
            }
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
                    
                    return StrategyFactory.GetStrategy(type,symbol,timeframe);

                }
            }

            throw new NotFoundException("La strategy n'existe pas");

        }
        
        private (DateTime start, DateTime? end) DateControl(string start, string? end)
        {
            DateTime dateStart = default;
            DateTime dateEnd = default;

            
            start = start ?? throw new FormatDateException($"L'argument start ne doit pas être vide");
            if (!DateTime.TryParse(start, out dateStart))
            {
                throw new FormatDateException("La start date n'est pas au bon format");
            }
            if (end is not null)
            {
                if (!DateTime.TryParse(end, out dateEnd))
                {
                    throw new FormatDateException("La start date n'est pas au bon format");
                }
                if (dateEnd < dateStart)
                {
                    throw new InvalideDateRangeException("La end date doit être supérieur à la start date");
                }
                return (dateStart,dateEnd);
            }
            return (dateStart,null);

        }
    }
}