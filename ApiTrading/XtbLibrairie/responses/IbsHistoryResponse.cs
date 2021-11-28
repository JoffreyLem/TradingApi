namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class IbsHistoryResponse : BaseResponse
    {
        public IbsHistoryResponse(string body)
            : base(body)
        {
            var arr = (JSONArray)ReturnData;

            foreach (JSONObject e in arr)
            {
                var record = new IbRecord(e);
                IbRecords.AddLast(record);
            }
        }

        /// <summary>
        ///     IB records.
        /// </summary>
        public LinkedList<IbRecord> IbRecords { get; set; }
    }
}