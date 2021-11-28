namespace ApiTrading.Exception
{
    using System.Collections.Generic;

    public class AlreadyExistException : CustomErrorException
    {
        public AlreadyExistException(List<string> errorMessage) : base(errorMessage)
        {
        }

        public AlreadyExistException(string message) : base(message)
        {
        }
    }
}