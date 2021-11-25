using System.Collections.Generic;

namespace ApiTrading.Exception
{
    public class PasswordUpdateException: CustomErrorException

    {
        public PasswordUpdateException(List<string> errorsMessages) : base(errorsMessages)
        {
        }

        public PasswordUpdateException(string? message) : base(message)
        {
        }
    }
}