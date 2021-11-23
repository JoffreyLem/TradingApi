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
      


        public abstract void Ping();
        public abstract SymbolInformations? GetSymbolInformation(string symbol);
        public abstract void GetAllSymbol();

        public SymbolInformations RequestSymbol(string symbol)
        {
            return AllSymbolList.FirstOrDefault(x => x.Symbol == symbol);
        }

       

        public abstract Task<BaseResponse<List<Candle>>> GetAllChart(string symbol, string periodCodeStr, double? symbolTickSize,
            bool fullData = true);

        public abstract Task<BaseResponse<List<Candle>>> GetPartialChart(string symbol, string periodCodeStr, double? symbolTickSize,
            long? start, long? end);

        public abstract Task<BaseResponse<AccountInfo>> GetAccountInfo();
      
    }
}