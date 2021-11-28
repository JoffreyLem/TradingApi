namespace ApiTrading.Service.ExternalAPIHandler
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Modele;
    using Modele.DTO.Response;
    using XtbLibrairie.sync;

    public interface IApiHandler
    {
        public List<SymbolInformations> AllSymbolList { get; set; }
        public SyncAPIConnector connector { get; set; }

        public Task<BaseResponse> Login(string user, string password);

        public Task<BaseResponse> Logout();


        public Task<BaseResponse<List<SymbolResponse>>> GetAllSymbol();

        public Task<BaseResponse<bool>> CheckIfSymbolExist(string symbol);


        public Task<BaseResponse<List<Candle>>> GetAllChart(string symbol, string periodCodeStr,
            bool fullData = true);

        public Task<BaseResponse<List<Candle>>> GetPartialChart(string symbol, string periodCodeStr,
            string? start, string? end);

        public Task<BaseResponse<List<Candle>>> GetChart(string symbol, string periodCodeStr,
            string? start, string? end);
    }
}