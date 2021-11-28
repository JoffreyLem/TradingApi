namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class BalanceRecordsStop
    {
        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "stopBalance");
            return result.ToString();
        }
    }
}