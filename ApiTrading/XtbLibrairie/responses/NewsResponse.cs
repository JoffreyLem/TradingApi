using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    using JSONArray = JArray;
    using JSONObject = JObject;

    public class NewsResponse : BaseResponse
    {
        private readonly LinkedList<NewsTopicRecord> newsTopicRecords = new LinkedList<NewsTopicRecord>();

        public NewsResponse(string body) : base(body)
        {
            var arr = (JSONArray) ReturnData;
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