namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class CalendarResponse : BaseResponse
    {
        public CalendarResponse(string body)
            : base(body)
        {
            var returnData = (JSONArray)ReturnData;

            foreach (JSONObject e in returnData)
            {
                var record = new CalendarRecord();
                record.FieldsFromJSONObject(e);
                CalendarRecords.Add(record);
            }
        }

        public List<CalendarRecord> CalendarRecords { get; } = new();
    }
}