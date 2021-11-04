namespace AlphavantageWrapper
{
    public class StockTimeSeriesParam : ApiBaseParam
    {
        public string Interval { get; set; }
        public StockTimeSeriesParam(string symbol, string function) : base(symbol, function)
        {
            
        }
    }
}