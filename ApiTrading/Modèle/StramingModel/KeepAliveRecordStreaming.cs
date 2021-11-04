using Newtonsoft.Json.Linq;

namespace Modele.StramingModel
{
    using JSONObject = JObject;

    public class KeepAliveRecordStreaming
    {
        public long? Timestamp { get; set; }

        public void XtbParsing(JSONObject value)
        {
            Timestamp = (long?) value["timestamp"];
        }

        public override string ToString()
        {
            return "StreamingKeepAliveRecord{" +
                   "timestamp=" + Timestamp +
                   '}';
        }
    }
}