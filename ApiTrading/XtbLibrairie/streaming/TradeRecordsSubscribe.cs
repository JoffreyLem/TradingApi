namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class TradeRecordsSubscribe
    {
        private readonly string streamSessionId;

        public TradeRecordsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "getTrades");
            result.Add("streamSessionId", streamSessionId);
            return result.ToString();
        }
    }
}