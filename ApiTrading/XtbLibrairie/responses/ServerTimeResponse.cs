using Newtonsoft.Json.Linq;

namespace XtbLibrairie.responses
{
    using JSONObject = JObject;

    public class ServerTimeResponse : BaseResponse
    {
        public ServerTimeResponse(string body) : base(body)
        {
            var ob = (JSONObject) ReturnData;
            Time = (long?) ob["time"];
            TimeString = (string) ob["timeString"];
        }

        public virtual long? Time { get; }

        public virtual string TimeString { get; }
    }
}