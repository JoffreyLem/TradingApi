namespace Strategy.Exception
{
    using System;

    public class InvalidTakeProfitException : Exception
    {
        public InvalidTakeProfitException(string message) : base(message)
        {
        }
    }
}