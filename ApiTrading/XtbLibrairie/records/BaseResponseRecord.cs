using Newtonsoft.Json.Linq;

namespace XtbLibrairie.records
{
    using JSONObject = JObject;

    public interface BaseResponseRecord
    {
        void FieldsFromJSONObject(JSONObject value);
    }
}