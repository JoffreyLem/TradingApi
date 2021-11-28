namespace XtbLibrairie.records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public interface BaseResponseRecord
    {
        void FieldsFromJSONObject(JSONObject value);
    }
}