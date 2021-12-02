using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    using JSONArray = JArray;
    using JSONObject = JObject;

    public class AllSymbolsResponse : BaseResponse
    {
        private readonly LinkedList<SymbolRecord> symbolRecords = new();

        public AllSymbolsResponse(string body) : base(body)
        {
            var symbolRecords = (JSONArray) ReturnData;
            foreach (JSONObject e in symbolRecords)
            {
                var symbolRecord = new SymbolRecord();
                symbolRecord.FieldsFromJSONObject(e);
                this.symbolRecords.AddLast(symbolRecord);
            }
        }

        public virtual LinkedList<SymbolRecord> SymbolRecords => symbolRecords;
    }
}