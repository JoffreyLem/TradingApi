namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class TradeStatusRecordsStop
    {
        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "stopTradeStatus");
            return result.ToString();
        }
    }
}