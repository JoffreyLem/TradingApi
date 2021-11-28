namespace XtbLibrairie.responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ConfirmPricedResponse : BaseResponse
    {
        public ConfirmPricedResponse(string body) : base(body)
        {
            var ob = (JSONObject)ReturnData;
            NewRequestId = (long?)ob["requestId"];
        }

        public virtual long? NewRequestId { get; }
    }
}