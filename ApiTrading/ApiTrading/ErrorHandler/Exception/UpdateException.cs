using System.Collections.Generic;

namespace ApiTrading.Exception
{
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