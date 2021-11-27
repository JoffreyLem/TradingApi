using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Response;

namespace ApiTrading.Service.Strategy
{
    using Microsoft.AspNetCore.Identity;

    public interface IStrategyService
    {
        public Task<StrategyResponse> GetAllStrategy();

        public Task<TimeframeResponse> GetAllTimeframe();

        public Task<SignalResponse> GetSignals(string strategy, string symbol, string timeframe,IdentityUser<int> user = null);
        
      
    }
}