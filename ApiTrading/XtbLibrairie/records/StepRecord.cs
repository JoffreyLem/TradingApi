namespace XtbLibrairie.records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class StepRecord : BaseResponseRecord
    {
        private double FromValue;
        private double Step;

        public void FieldsFromJSONObject(JSONObject value)
        {
            FromValue = (double)value["fromValue"];
            Step = (double)value["step"];
        }
    }
}