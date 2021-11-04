using Newtonsoft.Json.Linq;

namespace Modele.StramingModel
{
    using JSONObject = JObject;

    public class ProfitRecordStreaming
    {
        public long? Order { get; set; }

        public long? Order2 { get; set; }

        public long? Position { get; set; }

        public double? Profit { get; set; }

        public void XtbParsing(JSONObject value)
        {
            Profit = (double?) value["profit"];
            Order = (long?) value["order"];
        }

        public override string ToString()
        {
            return "StreamingProfitRecord{" +
                   "profit=" + Profit +
                   ", order=" + Order +
                   '}';
        }
    }
}