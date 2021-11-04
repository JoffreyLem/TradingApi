using System;
using Newtonsoft.Json.Linq;

namespace XtbLibrairie.responses
{
    using JSONObject = JObject;

    public class TradeTransactionResponse : BaseResponse
    {
        public TradeTransactionResponse(string body) : base(body)
        {
            var ob = (JSONObject) ReturnData;
            Order = (long?) ob["order"];
        }

        [Obsolete("Use Order instead")] public virtual long? RequestId => Order;

        public long? Order { get; }
    }
}