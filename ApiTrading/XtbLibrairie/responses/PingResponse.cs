using Newtonsoft.Json.Linq;

namespace XtbLibrairie.responses
{
    using JSONObject = JObject;

    public class PingResponse : BaseResponse
    {
        private long? time;
        private string timeString;

        public PingResponse(string body) : base(body)
        {
            var ob = (JSONObject) ReturnData;
        }
    }
}