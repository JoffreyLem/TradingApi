using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    using JSONArray = JArray;
    using JSONObject = JObject;

    public class TradingHoursResponse : BaseResponse
    {
        private readonly LinkedList<TradingHoursRecord> tradingHoursRecords = new LinkedList<TradingHoursRecord>();

        public TradingHoursResponse(string body) : base(body)
        {
            var ob = (JSONArray) ReturnData;
            foreach (JSONObject e in ob)
            {
                var record = new TradingHoursRecord();
                record.FieldsFromJSONObject(e);
                tradingHoursRecords.AddLast(record);
            }
        }

        public virtual LinkedList<TradingHoursRecord> TradingHoursRecords => tradingHoursRecords;
    }
}