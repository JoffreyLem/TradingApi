namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class AllSymbolsResponse : BaseResponse
    {
        private readonly LinkedList<SymbolRecord> symbolRecords = new();

        public AllSymbolsResponse(string body) : base(body)
        {
            var symbolRecords = (JSONArray)ReturnData;
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