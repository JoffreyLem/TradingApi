namespace ApiTrading.Exception
{
    using System.Collections.Generic;

    public class InvalideDateRangeException : CustomErrorException
    {
        public InvalideDateRangeException(List<string> errorsMessages) : base(errorsMessages)
        {
        }

        public InvalideDateRangeException(string? message) : base(message)
        {
        }
    }
}