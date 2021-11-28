namespace ApiTrading.Exception
{
    using System.Collections.Generic;

    public class PasswordUpdateException : CustomErrorException

    {
        public PasswordUpdateException(List<string> errorsMessages) : base(errorsMessages)
        {
        }

        public PasswordUpdateException(string? message) : base(message)
        {
        }
    }
}