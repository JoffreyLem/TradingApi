namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class ProfitsStop
    {
        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "stopProfits");
            return result.ToString();
        }
    }
}