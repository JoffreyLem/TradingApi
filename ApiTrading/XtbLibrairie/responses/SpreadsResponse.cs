namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class SpreadsResponse : BaseResponse
    {
        private readonly LinkedList<SpreadRecord> spreadRecords = new();

        public SpreadsResponse(string body)
            : base(body)
        {
            var symbolRecords = (JSONArray)ReturnData;
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