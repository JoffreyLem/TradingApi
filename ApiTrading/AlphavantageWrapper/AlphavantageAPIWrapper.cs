using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIhandler;
using Modele;
using Modele.StramingModel;

namespace AlphavantageWrapper
{
    public class AlphavantageApiWrapper : ApiHandler
    {
        public override string ApiName { get => "Alphavantage"; }
        public override void ConnectStreaming()
        {
            throw new NotImplementedException();
        }

        public override void DisConnectStreaming()
        {
            throw new NotImplementedException();
        }

        public override void SubscribeCandle(string symbol)
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeCandle(string symbol)
        {
            throw new NotImplementedException();
        }

        public override void SubscribeBalance()
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeBalance()
        {
            throw new NotImplementedException();
        }

        public override void SubscribeNews()
        {
            throw new NotImplementedException();
        }

        public override void UnSubscribeNews()
        {
            throw new NotImplementedException();
        }

        public override void SubscribeTrade()
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeTrade()
        {
            throw new NotImplementedException();
        }

        public override void SubscribeTradeStatus()
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeTradeStatus()
        {
            throw new NotImplementedException();
        }

        public override void SubscribeProfit()
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeProfit()
        {
            throw new NotImplementedException();
        }

        public override void SubscribePrice(string symbol)
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribePrice(string symbol)
        {
            throw new NotImplementedException();
        }

        public override void SetCredentials()
        {
            throw new NotImplementedException();
        }

        public override void AskForCredentials()
        {
            throw new NotImplementedException();
        }

        public override void Ping()
        {
            throw new NotImplementedException();
        }

        public override SymbolInformations GetSymbolInformation(string symbol)
        {
            throw new NotImplementedException();
        }

        public override void GetAllSymbol()
        {
            throw new NotImplementedException();
        }

        public override double? CalculerProfit(string symbol, double? volume, TypePosition typePosition, double? openPrice, double? closePrice)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Candle>> GetAllChart(string symbol, string periodCodeStr, double? symbolTickSize, bool fullData = false)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Candle>> GetPartialChart(string symbol, string periodCodeStr, double? symbolTickSize, long? start, long? end)
        {
            throw new NotImplementedException();
        }

        public override Task<ApiResponse> OpenPosition(TypePosition cmd, double? price, double? sl, double? tp, string symbol, double? volume, long? order,
            long? expiration, string customComment)
        {
            throw new NotImplementedException();
        }

        public override Task<ApiResponse> UpdatePosition(Position tradeRecord, PositionInformations positionInformations)
        {
            throw new NotImplementedException();
        }

        public override Task<ApiResponse> ClosePosition(Position tradeRecord, PositionInformations positionInformations)
        {
            throw new NotImplementedException();
        }

        public override Task<AccountInfo> GetAccountInfo()
        {
            throw new NotImplementedException();
        }

        public override Task<List<Position>> GetTradeHistory(DateTime start)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Position>> GetTrades()
        {
            throw new NotImplementedException();
        }

        public override void AddCallBackCandleReceived(Action<CandleRecordStreaming> streamingOnTradeRecordReceived)
        {
            throw new NotImplementedException();
        }

        public override void AddCallBackTradeBalanceReceived(Action<BalanceRecordStreaming> streamingOnTradeRecordReceived)
        {
            throw new NotImplementedException();
        }

        public override void AddCallBackNewsReceived(Action<NewsRecordStreaming> streamingOnTradeRecordReceived)
        {
            throw new NotImplementedException();
        }

        public override void AddCallBackTradeRecordReceived(Action<TradeRecordStreaming> streamingOnTradeRecordReceived)
        {
            throw new NotImplementedException();
        }

        public override void AddCallBackTradeStatusRecordReceived(Action<TradeStatusStreaming> streamingOnTradeRecordReceived)
        {
            throw new NotImplementedException();
        }

        public override void AddCallBackProfitReceived(Action<ProfitRecordStreaming> streamingOnTradeRecordReceived)
        {
            throw new NotImplementedException();
        }

        public override void AddCallBackPriceReceived(Action<TickRecordStreaming> streamingOnTradeRecordReceived)
        {
            throw new NotImplementedException();
        }
    }
}