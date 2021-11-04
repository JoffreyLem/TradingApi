namespace APIhandler
{
    public interface IStreamingApiHandler
    {
        public void ConnectStreaming();
        public void DisConnectStreaming();
        public void SubscribeCandle(string symbol);
        public void UnsubscribeCandle(string symbol);
        public void SubscribeBalance();
        public void UnsubscribeBalance();
        public void SubscribeNews();
        public void UnSubscribeNews();
        public void SubscribeTrade();
        public void UnsubscribeTrade();
        public void SubscribeTradeStatus();
        public void UnsubscribeTradeStatus();
        public void SubscribeProfit();
        public void UnsubscribeProfit();
        public void SubscribePrice(string symbol);
        public void UnsubscribePrice(string symbol);
    }
}