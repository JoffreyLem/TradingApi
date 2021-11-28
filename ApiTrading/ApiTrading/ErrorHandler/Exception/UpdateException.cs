namespace ApiTrading.Exception
{
    using System.Collections.Generic;

    public class UpdateException : CustomErrorException

    {
        public UpdateException(List<string> errorsMessages) : base(errorsMessages)
        {
        }

        public UpdateException(string? message) : base(message)
        {
        }
    }
}