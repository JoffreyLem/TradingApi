namespace ApiTrading.Service.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Modele;
    using Modele.DTO.Response;

    public interface IDataService
    {
        public abstract Task<BaseResponse<List<Candle>>> GetAllChart(string symbol, string periodCodeStr, double? symbolTickSize,
            bool fullData = true);

        public abstract Task<BaseResponse<List<Candle>>> GetPartialChart(string symbol, string periodCodeStr, double? symbolTickSize,
            long? start, long? end);
    }
}