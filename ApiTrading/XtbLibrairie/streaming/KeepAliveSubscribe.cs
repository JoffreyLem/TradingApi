namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class KeepAliveSubscribe
    {
        private readonly string streamSessionId;

        public KeepAliveSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "getKeepAlive");
            result.Add("streamSessionId", streamSessionId);
            return result.ToString();
        }
    }
}