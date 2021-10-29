using System;
using System.Globalization;

namespace ApiTrading.Exception
{
    public class ApplicationException : System.Exception
    {
        public ApplicationException() : base() {}

        public ApplicationException(string message) : base(message) { }

        public ApplicationException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}