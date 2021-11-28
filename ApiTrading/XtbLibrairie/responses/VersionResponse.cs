namespace XtbLibrairie.responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class VersionResponse : BaseResponse
    {
        public VersionResponse(string body)
            : base(body)
        {
            var returnData = (JSONObject)ReturnData;
            Version = (string)returnData["version"];
        }

        public virtual string Version { get; }
    }
}