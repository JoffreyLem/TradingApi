using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Response;
using Modele;
using XtbLibrairie.sync;

namespace ApiTrading.Service.ExternalAPIHandler
{
    public interface IApiHandler
    {


        public List<SymbolInformations> AllSymbolList { get; set; }

        public Task<BaseResponse> Login(string user, string password);

        public Task<BaseResponse> Logout();
        public  SyncAPIConnector connector { get; set; }
      


        public abstract Task<BaseResponse<List<SymbolResponse>>> GetAllSymbol();

        public Task<BaseResponse<bool>> CheckIfSymbolExist(string symbol);



        public abstract Task<BaseResponse<List<Candle>>> GetAllChart(string symbol, string periodCodeStr, 
            bool fullData = true);

        public abstract Task<BaseResponse<List<Candle>>> GetPartialChart(string symbol, string periodCodeStr, 
            string? start, string? end);

        public abstract Task<BaseResponse<List<Candle>>> GetChart(string symbol, string periodCodeStr,
            string? start, string? end);


      
    }
}