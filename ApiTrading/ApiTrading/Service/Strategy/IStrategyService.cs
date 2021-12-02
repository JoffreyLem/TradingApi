using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Request;
using ApiTrading.Modele.DTO.Response;
using Modele;

namespace ApiTrading.Service.Strategy
{
    public interface IStrategyService
    {
        public Task<BaseResponse<List<StrategyList>>> GetAllStrategy();

        public Task<BaseResponse<List<string>>> GetAllTimeframe();

        public Task<BaseResponse<List<SignalInfoStrategy>>> GetSignals(string strategy, string symbol, string timeframe,
            string user = null);

        public Task<BaseResponse> PostSignal(SignalInfoRequest infoRequest);

        public Task<BaseResponse<List<string>>> GetUsersGiverSignal();

        public Task<BaseResponse> SubscribeToSymbolInfo(string modelUser, string symbol);

        public Task<BaseResponse> UnsubscribeToSymbolInfo(string symbol);


        public Task<BaseResponse<List<SubscriptionResponse>>> GetCurrentSignalSubscription();
    }
}