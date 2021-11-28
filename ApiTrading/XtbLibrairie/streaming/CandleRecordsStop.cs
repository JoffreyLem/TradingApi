namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class CandleRecordsStop
    {
        private readonly string symbol;

        public CandleRecordsStop(string symbol)
        {
            this.symbol = symbol;
        }

        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "stopCandles");
            result.Add("symbol", symbol);
            return result.ToString();
        }
    }
}