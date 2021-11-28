namespace XtbLibrairie.streaming
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    internal class TickPricesStop
    {
        private readonly string symbol;

        public TickPricesStop(string symbol)
        {
            this.symbol = symbol;
        }

        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "stopTickPrices");
            result.Add("symbol", symbol);
            return result.ToString();
        }
    }
}