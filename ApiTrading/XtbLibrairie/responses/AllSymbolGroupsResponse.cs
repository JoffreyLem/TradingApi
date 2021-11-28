namespace XtbLibrairie.responses
{
    using System.Collections.Generic;
    using records;

    public class AllSymbolGroupsResponse : BaseResponse
    {
        public AllSymbolGroupsResponse(string body) : base(body)
        {
        }

        public virtual LinkedList<SymbolGroupRecord> SymbolGroupRecords { get; } = new();
    }
}