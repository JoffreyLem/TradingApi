using Newtonsoft.Json.Linq;

namespace Modele.StramingModel
{
    using JSONObject = JObject;

    public class NewsRecordStreaming
    {
        public string Body { get; set; }

        public string Key { get; set; }

        public long? Time { get; set; }

        public string Title { get; set; }

        public void XtbParsing(JSONObject value)
        {
            Body = (string) value["body"];
            Key = (string) value["key"];
            Time = (long?) value["time"];
            Title = (string) value["title"];
        }
    }
}