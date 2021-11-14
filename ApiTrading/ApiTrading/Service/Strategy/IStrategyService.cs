using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Response;

namespace ApiTrading.Service.Strategy
{
    public interface IStrategyService
    {
        public Task<List<StrategyResponse>> GetAllStrategy();

        public Task<List<string>> GetAllTimeframe();
    }
}