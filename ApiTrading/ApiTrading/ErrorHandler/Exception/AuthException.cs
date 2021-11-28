namespace ApiTrading.Exception
{
    using System.Collections.Generic;

    public class AuthException : CustomErrorException
    {
        public AuthException(string? message) : base(message)
        {
        }

        public AuthException(List<string> errorMessages) : base(errorMessages)
        {
        }
    }
}