namespace XtbLibrairie.errors
{
    using System;

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