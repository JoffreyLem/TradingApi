namespace ApiTrading.Service.Strategy
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Modele;
    using Microsoft.AspNetCore.Identity;
    using Modele;
    using Modele.DTO.Request;
    using Modele.DTO.Response;

    public interface IStrategyService
    {
        public Task<BaseResponse<List<StrategyList>>> GetAllStrategy();

        public Task<BaseResponse<List<string>>> GetAllTimeframe();

        public Task<BaseResponse<List<SignalInfoStrategy>>> GetSignals(string strategy, string symbol, string timeframe,
            string user = null);

        public Task<BaseResponse> PostSignal(SignalInfoRequest infoRequest);

        public Task<BaseResponse<List<string>>> GetUsersGiverSignal();

        public Task<BaseResponse> SubscribeToSymbolInfo(string symbol);

        public Task<BaseResponse> UnsubscribeToSymbolInfo(string symbol);
        
   

        public Task<BaseResponse<List<Subscription>>> GetCurrentSignalSubscription();
    }
}