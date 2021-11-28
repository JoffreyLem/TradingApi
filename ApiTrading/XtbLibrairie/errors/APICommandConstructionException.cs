namespace XtbLibrairie.errors
{
    using System;

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