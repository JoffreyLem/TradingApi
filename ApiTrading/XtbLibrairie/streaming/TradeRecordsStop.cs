namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class TradeRecordsStop
    {
        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "stopTrades");
            return result.ToString();
        }
    }
}