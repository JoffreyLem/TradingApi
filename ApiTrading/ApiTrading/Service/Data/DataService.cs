using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Response;
using Modele;

namespace ApiTrading.Service.Data
{
    public class DataService : IDataService
    {
        public Task<BaseResponse<List<Candle>>> GetAllChart(string symbol, string periodCodeStr, double? symbolTickSize,
            bool fullData = true)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<List<Candle>>> GetPartialChart(string symbol, string periodCodeStr,
            double? symbolTickSize, long? start, long? end)
        {
            throw new NotImplementedException();
        }
    }
}