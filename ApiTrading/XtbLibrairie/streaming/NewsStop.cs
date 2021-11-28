namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class NewsStop
    {
        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "stopNews");
            return result.ToString();
        }
    }
}