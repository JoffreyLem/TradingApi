namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class NewsResponse : BaseResponse
    {
        private readonly LinkedList<NewsTopicRecord> newsTopicRecords = new();

        public NewsResponse(string body) : base(body)
        {
            var arr = (JSONArray)ReturnData;
            foreach (JSONObject e in arr)
            {
                var record = new NewsTopicRecord();
                record.FieldsFromJSONObject(e);
                newsTopicRecords.AddLast(record);
            }
        }

        public virtual LinkedList<NewsTopicRecord> NewsTopicRecords => newsTopicRecords;
    }
}