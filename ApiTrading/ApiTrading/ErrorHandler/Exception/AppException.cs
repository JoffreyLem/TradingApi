namespace ApiTrading.Exception
{
    using System.Collections.Generic;

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