namespace XtbLibrairie.responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class MarginTradeResponse : BaseResponse
    {
        public MarginTradeResponse(string body) : base(body)
        {
            var ob = (JSONObject)ReturnData;
            Margin = (double?)ob["margin"];
        }

        public virtual double? Margin { get; }
    }
}