namespace XtbLibrairie.records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class HoursRecord : BaseResponseRecord
    {
        private long? day;
        private long? fromT;
        private long? toT;

        public virtual long? Day => day;

        public virtual long? FromT => fromT;

        public virtual long? ToT => toT;

        public void FieldsFromJSONObject(JSONObject value)
        {
            day = (long?)value["day"];
            fromT = (long?)value["fromT"];
            toT = (long?)value["toT"];
        }

        public override string ToString()
        {
            return "HoursRecord{" + "day=" + day + ", fromT=" + fromT + ", toT=" + toT + '}';
        }
    }
}