using System;

namespace XtbLibrairie.errors
{
    public class APICommandConstructionException : Exception
    {
        public APICommandConstructionException()
        {
        }

        public APICommandConstructionException(string msg) : base(msg)
        {
        }
    }
}