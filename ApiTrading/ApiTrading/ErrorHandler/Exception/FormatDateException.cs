namespace ApiTrading.Exception
{
    using System.Collections.Generic;

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