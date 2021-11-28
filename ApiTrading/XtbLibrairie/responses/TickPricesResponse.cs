namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TickPricesResponse : BaseResponse
    {
        private readonly LinkedList<TickRecord> ticks = new();

        public TickPricesResponse(string body) : base(body)
        {
            var ob = (JSONObject)ReturnData;
            var arr = (JSONArray)ob["quotations"];
            foreach (JSONObject e in arr)
            {
                var record = new TickRecord();
                record.FieldsFromJSONObject(e);
                ticks.AddLast(record);
            }
        }

        public virtual LinkedList<TickRecord> Ticks => ticks;
    }
}