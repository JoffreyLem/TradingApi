using System.Collections.Generic;

namespace ApiTrading.Exception
{
    public class CustomErrorException : System.Exception
    {
        public CustomErrorException(List<string> errorsMessages)
        {
            ErrorsMessages = new List<string>();
            foreach (var errorsMessage in errorsMessages) ErrorsMessages.Add(errorsMessage);
        }

        protected CustomErrorException(string? message)
        {
            ErrorsMessages = new List<string>();
            ErrorsMessages.Add(message);
        }

        public List<string> ErrorsMessages { get; set; }
    }
}