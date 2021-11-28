namespace ApiTrading.Exception
{
    using System;
    using System.Collections.Generic;

    public class CustomErrorException : Exception
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