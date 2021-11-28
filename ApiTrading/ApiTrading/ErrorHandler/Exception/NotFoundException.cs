namespace ApiTrading.Exception
{
    using System.Collections.Generic;

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