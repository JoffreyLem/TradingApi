using System.Collections.Generic;

namespace ApiTrading.Exception
{
    public class AuthException : CustomErrorException
    {
        public AuthException(string? message) :base(message)
        {
        }

        public AuthException(List<string> errorMessages) : base(errorMessages)
        {
            
        }
    }
}