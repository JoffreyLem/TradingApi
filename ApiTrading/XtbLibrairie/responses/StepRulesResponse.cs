using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    using JSONArray = JArray;
    using JSONObject = JObject;

    public class StepRulesResponse : BaseResponse
    {
        private readonly LinkedList<StepRuleRecord> stepRulesRecords = new LinkedList<StepRuleRecord>();

        public StepRulesResponse(string body)
            : base(body)
        {
            var stepRulesRecords = (JSONArray) ReturnData;
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