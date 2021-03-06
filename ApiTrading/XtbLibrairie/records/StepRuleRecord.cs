using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace XtbLibrairie.records
{
    using JSONArray = JArray;
    using JSONObject = JObject;

    public class StepRuleRecord : BaseResponseRecord
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private LinkedList<StepRecord> Steps { get; set; }

        public void FieldsFromJSONObject(JSONObject value)
        {
            Id = (int) value["id"];
            Name = (string) value["name"];

            Steps = new LinkedList<StepRecord>();
            if (value["steps"] != null)
            {
                var jsonarray = (JSONArray) value["steps"];
                foreach (JSONObject i in jsonarray)
                {
                    var rec = new StepRecord();
                    rec.FieldsFromJSONObject(i);
                    Steps.AddLast(rec);
                }
            }
        }
    }
}