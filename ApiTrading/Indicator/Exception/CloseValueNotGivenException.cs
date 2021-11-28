namespace Indicator.Exception
{
    using System;

    public class CloseValueNotGivenException : Exception
    {
        public CloseValueNotGivenException(string? message) : base(message)
        {
        }
    }
}