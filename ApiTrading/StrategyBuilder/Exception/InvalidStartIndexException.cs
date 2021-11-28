namespace Strategy.Exception
{
    using System;

    public class InvalidStartIndexException : Exception
    {
        public InvalidStartIndexException(string? message) : base(message)
        {
        }
    }
}