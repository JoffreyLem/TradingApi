namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class StepRulesResponse : BaseResponse
    {
        private readonly LinkedList<StepRuleRecord> stepRulesRecords = new();

        public StepRulesResponse(string body)
            : base(body)
        {
            var stepRulesRecords = (JSONArray)ReturnData;
            foreach (JSONObject e in stepRulesRecords)
            {
                var stepRulesRecord = new StepRuleRecord();
                stepRulesRecord.FieldsFromJSONObject(e);
                this.stepRulesRecords.AddLast(stepRulesRecord);
            }
        }

        public virtual LinkedList<StepRuleRecord> StepRulesRecords => stepRulesRecords;
    }
}