namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class NewsSubscribe
    {
        private readonly string streamSessionId;

        public NewsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "getNews");
            result.Add("streamSessionId", streamSessionId);
            return result.ToString();
        }
    }
}