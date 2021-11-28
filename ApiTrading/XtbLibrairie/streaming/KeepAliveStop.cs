namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class KeepAliveStop
    {
        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "stopKeepAlive");
            return result.ToString();
        }
    }
}