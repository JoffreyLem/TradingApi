using Modele.StramingModel;

namespace XtbLibrairie.streaming
{
    public interface StreamingListener
    {
        void ReceiveTradeRecord(TradeRecordStreaming tradeRecord);
        void ReceiveTickRecord(TickRecordStreaming tickRecord);
        void ReceiveBalanceRecord(BalanceRecordStreaming balanceRecord);
        void ReceiveTradeStatusRecord(TradeStatusStreaming tradeStatusRecord);
        void ReceiveProfitRecord(ProfitRecordStreaming profitRecord);
        void ReceiveNewsRecord(NewsRecordStreaming newsRecord);
        void ReceiveKeepAliveRecord(KeepAliveRecordStreaming keepAliveRecord);
        void ReceiveCandleRecord(CandleRecordStreaming candleRecord);
    }
}