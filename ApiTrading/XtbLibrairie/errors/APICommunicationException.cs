using System;

namespace XtbLibrairie.errors
{
    public class APICommunicationException : Exception
    {
        public APICommunicationException()
        {
        }

        public APICommunicationException(string msg) : base(msg)
        {
        }
    }
}