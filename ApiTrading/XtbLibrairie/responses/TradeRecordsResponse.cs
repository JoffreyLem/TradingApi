using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    using JSONArray = JArray;
    using JSONObject = JObject;

    public class TradeRecordsResponse : BaseResponse
    {
        private readonly LinkedList<TradeRecord> tradeRecords = new();

        public TradeRecordsResponse(string body) : base(body)
        {
            var arr = (JSONArray) ReturnData;
            foreach (JSONObject e in arr)
            {
                var record = new TradeRecord();
                record.FieldsFromJSONObject(e);
                tradeRecords.AddLast(record);
            }
        }

        public virtual LinkedList<TradeRecord> TradeRecords => tradeRecords;
    }
}