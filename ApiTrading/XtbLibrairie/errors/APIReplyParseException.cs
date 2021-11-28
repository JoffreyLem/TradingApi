namespace XtbLibrairie.errors
{
    using System;

    public class APIReplyParseException : Exception
    {
        public APIReplyParseException()
        {
        }

        public APIReplyParseException(string msg) : base(msg)
        {
        }
    }
}