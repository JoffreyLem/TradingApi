using System.Collections.Generic;

namespace ApiTrading.Exception
{
    public class AlreadyExistException : CustomErrorException
    {
        public AlreadyExistException(List<string> errorMessage):base(errorMessage)
        {
        }

        public AlreadyExistException(string message) : base(message)
        {
        }
    }
}