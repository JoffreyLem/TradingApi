using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ApiTrading.Exception
{
    public class CustomErrorException : System.Exception
    {
        public List<string> ErrorsMessages { get; set; }

        public CustomErrorException(List<string> errorsMessages)
        {
            ErrorsMessages = new List<string>();
            foreach (var errorsMessage in errorsMessages)
            {
                ErrorsMessages.Add(errorsMessage);
            }
        }

        protected CustomErrorException(string? message)
        {
            ErrorsMessages = new List<string>();
            ErrorsMessages.Add(message);
        }
    }
}