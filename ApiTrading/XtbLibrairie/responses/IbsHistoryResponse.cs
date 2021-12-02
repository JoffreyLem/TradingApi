using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    using JSONArray = JArray;
    using JSONObject = JObject;

    public class IbsHistoryResponse : BaseResponse
    {
        public IbsHistoryResponse(string body)
            : base(body)
        {
            var arr = (JSONArray) ReturnData;

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