using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    using JSONArray = JArray;
    using JSONObject = JObject;

    public class ChartRangeResponse : BaseResponse
    {
        private readonly LinkedList<RateInfoRecord> rateInfos = new();

        public ChartRangeResponse(string body) : base(body)
        {
            var rd = (JSONObject) ReturnData;
            Digits = (long?) rd["digits"];
            var arr = (JSONArray) rd["rateInfos"];
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