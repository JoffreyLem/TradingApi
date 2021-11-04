using System;

namespace XtbLibrairie.errors
{
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