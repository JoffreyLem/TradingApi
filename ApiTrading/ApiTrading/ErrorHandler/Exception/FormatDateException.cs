using System.Collections.Generic;

namespace ApiTrading.Exception
{
    public class FormatDateException : CustomErrorException
    {
        public FormatDateException(List<string> errorsMessages) : base(errorsMessages)
        {
        }

        public FormatDateException(string? message) : base(message)
        {
        }
    }
}