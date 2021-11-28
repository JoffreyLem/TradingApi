namespace XtbLibrairie.records
{
    using System.Collections.Generic;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class StepRuleRecord : BaseResponseRecord
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private LinkedList<StepRecord> Steps { get; set; }

        public void FieldsFromJSONObject(JSONObject value)
        {
            Id = (int)value["id"];
            Name = (string)value["name"];

            Steps = new LinkedList<StepRecord>();
            if (value["steps"] != null)
            {
                var jsonarray = (JSONArray)value["steps"];
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