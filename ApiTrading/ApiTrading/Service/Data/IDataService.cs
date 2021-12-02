using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Response;
using Modele;

namespace ApiTrading.Service.Data
{
    public interface IDataService
    {
        public Task<BaseResponse<List<Candle>>> GetAllChart(string symbol, string periodCodeStr, double? symbolTickSize,
            bool fullData = true);

        public Task<BaseResponse<List<Candle>>> GetPartialChart(string symbol, string periodCodeStr,
            double? symbolTickSize,
            long? start, long? end);
    }
}