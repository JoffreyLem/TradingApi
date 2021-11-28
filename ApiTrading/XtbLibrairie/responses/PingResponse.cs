namespace XtbLibrairie.responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class PingResponse : BaseResponse
    {
        private long? time;
        private string timeString;

        public PingResponse(string body) : base(body)
        {
            var ob = (JSONObject)ReturnData;
        }
    }
}