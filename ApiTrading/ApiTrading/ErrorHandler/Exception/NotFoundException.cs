using System.Collections.Generic;

namespace ApiTrading.Exception
{
    public class NotFoundException : CustomErrorException
    {
        public NotFoundException(string? message) : base(message)
        {
        }

        public NotFoundException(List<string> errorsMessages) : base(errorsMessages)
        {
        }
    }
}