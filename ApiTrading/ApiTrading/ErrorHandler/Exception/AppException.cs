using System.Collections.Generic;

namespace ApiTrading.Exception
{
    public class AppException : CustomErrorException
    {
        public AppException(string message) : base(message)
        {
        }

        public AppException(List<string> messageErrorList) : base(messageErrorList)
        {
        }
    }
}