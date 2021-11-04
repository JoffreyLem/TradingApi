using Newtonsoft.Json.Linq;

namespace XtbLibrairie.streaming
{
    using JSONObject = JObject;

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