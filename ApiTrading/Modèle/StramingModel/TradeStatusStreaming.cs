using Newtonsoft.Json.Linq;

namespace Modele.StramingModel
{
    using JSONObject = JObject;

    public class TradeStatusStreaming
    {
        public RequestStatus RequestStatus { get; set; }

        public string CustomComment { get; set; }

        public string Message { get; set; }

        public long? Order { get; set; }

        public double? Price { get; set; }


        public void XtbParsing(JSONObject value)
        {
            CustomComment = (string) value["customComment"];
            Message = (string) value["message"];
            Order = (long?) value["order"];
            Price = (double?) value["price"];
            RequestStatus = XtbModelHelper.GetRequestStatus((long) value["requestStatus"]);
        }

        public override string ToString()
        {
            return "StreamingTradeStatusRecord{" +
                   "customComment=" + CustomComment +
                   "message=" + Message +
                   ", order=" + Order +
                   ", requestStatus=" + RequestStatus +
                   ", price=" + Price +
                   '}';
        }
    }
}