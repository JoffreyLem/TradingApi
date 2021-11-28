namespace XtbLibrairie.records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class StreamingProfitRecord : BaseResponseRecord
    {
        public long? Order { get; set; }

        public long? Order2 { get; set; }

        public long? Position { get; set; }

        public double? Profit { get; set; }

        public void FieldsFromJSONObject(JSONObject value)
        {
            Profit = (double?)value["profit"];
            Order = (long?)value["order"];
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