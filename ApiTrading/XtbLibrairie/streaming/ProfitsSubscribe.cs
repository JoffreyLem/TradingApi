namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class ProfitsSubscribe
    {
        private readonly string streamSessionId;

        public ProfitsSubscribe(string streamSessionId)
        {
            this.streamSessionId = streamSessionId;
        }

        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "getProfits");
            result.Add("streamSessionId", streamSessionId);
            return result.ToString();
        }
    }
}