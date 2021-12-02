using System.Collections.Generic;

namespace ApiTrading.Exception
{
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