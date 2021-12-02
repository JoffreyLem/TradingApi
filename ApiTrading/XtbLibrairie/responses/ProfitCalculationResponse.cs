using Newtonsoft.Json.Linq;

namespace XtbLibrairie.responses
{
    using JSONObject = JObject;

    public class ProfitCalculationResponse : BaseResponse
    {
        public ProfitCalculationResponse(string body) : base(body)
        {
            var ob = (JSONObject) ReturnData;
            Profit = (double?) ob["profit"];
        }

        public virtual double? Profit { get; }
    }
}