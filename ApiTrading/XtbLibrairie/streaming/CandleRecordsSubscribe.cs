namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class CandleRecordsSubscribe
    {
        private readonly string streamSessionId;
        private readonly string symbol;

        public CandleRecordsSubscribe(string symbol, string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
            this.symbol = symbol;
        }

        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "getCandles");
            result.Add("streamSessionId", streamSessionId);
            result.Add("symbol", symbol);
            return result.ToString();
        }
    }
}