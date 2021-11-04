namespace AlphavantageWrapper
{
    public class ApiBaseParam
    {
        public string Symbol { get; set; }
        public string Function { get; set; }

        public ApiBaseParam(string symbol, string function)
        {
            Symbol = symbol;
            Function = function;
        }
    }
}