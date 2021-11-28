namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TradingHoursResponse : BaseResponse
    {
        private readonly LinkedList<TradingHoursRecord> tradingHoursRecords = new();

        public TradingHoursResponse(string body) : base(body)
        {
            var ob = (JSONArray)ReturnData;
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