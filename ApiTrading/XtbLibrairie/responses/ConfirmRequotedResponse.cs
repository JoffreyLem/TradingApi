namespace XtbLibrairie.responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ConfirmRequotedResponse : BaseResponse
    {
        public ConfirmRequotedResponse(string body) : base(body)
        {
            var ob = (JSONObject)ReturnData;
            NewRequestId = (long?)ob["requestId"];
        }

        public virtual long? NewRequestId { get; }
    }
}