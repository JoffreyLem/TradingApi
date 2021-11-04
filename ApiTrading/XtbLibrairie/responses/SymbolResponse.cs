using Newtonsoft.Json.Linq;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    using JSONObject = JObject;

    public class SymbolResponse : BaseResponse
    {
        private readonly SymbolRecord symbol;

        public SymbolResponse(string body) : base(body)
        {
            var ob = (JSONObject) ReturnData;
            symbol = new SymbolRecord();
            symbol.FieldsFromJSONObject(ob);
        }

        public virtual SymbolRecord Symbol => symbol;
    }
}