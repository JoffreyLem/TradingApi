namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class BalanceRecordsSubscribe
    {
        private readonly string streamSessionId;

        public BalanceRecordsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "getBalance");
            result.Add("streamSessionId", streamSessionId);
            return result.ToString();
        }
    }
}