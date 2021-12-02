using System.Collections.Generic;
using XtbLibrairie.records;

namespace XtbLibrairie.responses
{
    public class AllSymbolGroupsResponse : BaseResponse
    {
        public AllSymbolGroupsResponse(string body) : base(body)
        {
        }

        public virtual LinkedList<SymbolGroupRecord> SymbolGroupRecords { get; } = new();
    }
}