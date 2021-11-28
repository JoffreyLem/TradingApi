namespace XtbLibrairie.responses
{
    using records;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class SymbolResponse : BaseResponse
    {
        private readonly SymbolRecord symbol;

        public SymbolResponse(string body) : base(body)
        {
            var ob = (JSONObject)ReturnData;
            symbol = new SymbolRecord();
            symbol.FieldsFromJSONObject(ob);
        }

        public virtual SymbolRecord Symbol => symbol;
    }
}