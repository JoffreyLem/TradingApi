namespace XtbLibrairie.responses
{
    using records;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class LoginResponse : BaseResponse
    {
        private readonly RedirectRecord redirectRecord;

        public LoginResponse(string body)
            : base(body)
        {
            var ob = JSONObject.Parse(body);
            StreamSessionId = (string)ob["streamSessionId"];

            var redirectJSON = (JSONObject)ob["redirect"];

            if (redirectJSON != null)
            {
                redirectRecord = new RedirectRecord();
                redirectRecord.FieldsFromJSONObject(redirectJSON);
            }
        }

        public virtual string StreamSessionId { get; }

        public virtual RedirectRecord RedirectRecord => redirectRecord;
    }
}