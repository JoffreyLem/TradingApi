namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ChartLastResponse : BaseResponse
    {
        private readonly LinkedList<RateInfoRecord> rateInfos = new();

        public ChartLastResponse(string body) : base(body)
        {
            var rd = (JSONObject)ReturnData;
            Digits = (long?)rd["digits"];
            var arr = (JSONArray)rd["rateInfos"];

            foreach (JSONObject e in arr)
            {
                var record = new RateInfoRecord();
                record.FieldsFromJSONObject(e);
                rateInfos.AddLast(record);
            }
        }

        public virtual long? Digits { get; }

        public virtual LinkedList<RateInfoRecord> RateInfos => rateInfos;
    }
}