namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class TradeStatusRecordsSubscribe
    {
        private readonly string streamSessionId;

        public TradeStatusRecordsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "getTradeStatus");
            result.Add("streamSessionId", streamSessionId);
            return result.ToString();
        }
    }
}