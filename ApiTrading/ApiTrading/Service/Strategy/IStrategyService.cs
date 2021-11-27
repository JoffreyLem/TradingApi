using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Response;

namespace ApiTrading.Service.Strategy
{
    using global::Modele;
    using Microsoft.AspNetCore.Identity;

    public interface IStrategyService
    {
        public Task<BaseResponse<List<StrategyList>>> GetAllStrategy();

        public Task<BaseResponse<List<string>>> GetAllTimeframe();

        public Task<BaseResponse<List<SignalInfoStrategy>>> GetSignals(string strategy, string symbol, string timeframe,IdentityUser<int> user = null);
        
      
    }
}