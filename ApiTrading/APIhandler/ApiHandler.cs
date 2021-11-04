using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modele;
using Modele.StramingModel;

namespace APIhandler
{
    public abstract class ApiHandler : IStreamingApiHandler
    {
        public ApiHandler()
        {
            SetCredentials();
        }

        public abstract string ApiName { get; }

        public virtual string CredentialFileName => ApiName + "Credential.txt";

        public string CredentialFileFolder =>
            AppDomain.CurrentDomain.BaseDirectory + @"CredentialsFolder/" + CredentialFileName;

        public string CredentialFolder =>
            AppDomain.CurrentDomain.BaseDirectory + @"CredentialsFolder";

        public List<SymbolInformations> AllSymbolList { get; set; }


        public abstract void ConnectStreaming();
        public abstract void DisConnectStreaming();
        public abstract void SubscribeCandle(string symbol);
        public abstract void UnsubscribeCandle(string symbol);
        public abstract void SubscribeBalance();
        public abstract void UnsubscribeBalance();
        public abstract void SubscribeNews();
        public abstract void UnSubscribeNews();
        public abstract void SubscribeTrade();
        public abstract void UnsubscribeTrade();
        public abstract void SubscribeTradeStatus();
        public abstract void UnsubscribeTradeStatus();
        public abstract void SubscribeProfit();
        public abstract void UnsubscribeProfit();
        public abstract void SubscribePrice(string symbol);
        public abstract void UnsubscribePrice(string symbol);

        public abstract void SetCredentials();
        public abstract void AskForCredentials();

        public abstract void Ping();
        public abstract SymbolInformations GetSymbolInformation(string symbol);
        public abstract void GetAllSymbol();

        public SymbolInformations RequestSymbol(string symbol)
        {
            return AllSymbolList.FirstOrDefault(x => x.Symbol == symbol);
        }

        public abstract double? CalculerProfit(string symbol, double? volume, TypePosition typePosition,
            double? openPrice,
            double? closePrice);

        public abstract Task<List<Candle>> GetAllChart(string symbol, string periodCodeStr, double? symbolTickSize,
            bool fullData = false);

        public abstract Task<List<Candle>> GetPartialChart(string symbol, string periodCodeStr, double? symbolTickSize,
            long? start, long? end);

        public abstract Task<ApiResponse> OpenPosition(TypePosition cmd, double? price, double? sl, double? tp,
            string symbol,
            double? volume, long? order, long? expiration, string customComment);

        public abstract Task<ApiResponse> UpdatePosition(Position tradeRecord,
            PositionInformations positionInformations);

        public abstract Task<ApiResponse>
            ClosePosition(Position tradeRecord, PositionInformations positionInformations);

        public abstract Task<AccountInfo> GetAccountInfo();
        public abstract Task<List<Position>> GetTradeHistory(DateTime start);
        public abstract Task<List<Position>> GetTrades();
        public abstract void AddCallBackCandleReceived(Action<CandleRecordStreaming> streamingOnTradeRecordReceived);

        public abstract void AddCallBackTradeBalanceReceived(
            Action<BalanceRecordStreaming> streamingOnTradeRecordReceived);

        public abstract void AddCallBackNewsReceived(Action<NewsRecordStreaming> streamingOnTradeRecordReceived);

        public abstract void
            AddCallBackTradeRecordReceived(Action<TradeRecordStreaming> streamingOnTradeRecordReceived);

        public abstract void AddCallBackTradeStatusRecordReceived(
            Action<TradeStatusStreaming> streamingOnTradeRecordReceived);

        public abstract void AddCallBackProfitReceived(Action<ProfitRecordStreaming> streamingOnTradeRecordReceived);
        public abstract void AddCallBackPriceReceived(Action<TickRecordStreaming> streamingOnTradeRecordReceived);
    }
}