using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    using JSONArray = JArray;
    using JSONObject = JObject;

    public class AllSpreadsResponse : BaseResponse
    {
        private readonly LinkedList<SpreadRecord> spreadRecords = new();

        public AllSpreadsResponse(string body) : base(body)
        {
            var symbolRecords = (JSONArray) ReturnData;
            foreach (JSONObject e in symbolRecords)
            {
                var spreadRecord = new SpreadRecord();
                spreadRecord.FieldsFromJSONObject(e);
                spreadRecords.AddLast(spreadRecord);
            }
        }

        public virtual LinkedList<SpreadRecord> SpreadRecords => spreadRecords;
    }
}