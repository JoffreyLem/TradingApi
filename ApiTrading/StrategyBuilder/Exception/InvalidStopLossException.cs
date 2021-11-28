namespace Strategy.Exception
{
    using System;

    public class InvalidStopLossException : Exception
    {
        public InvalidStopLossException(string message) : base(message)
        {
        }
    }
}