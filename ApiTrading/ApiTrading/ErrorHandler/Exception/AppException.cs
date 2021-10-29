using System;
using System.Collections.Generic;
using System.Globalization;

namespace ApiTrading.Exception
{
    public class AppException : CustomErrorException
    {
        public AppException(string message) :base(message)
        {

        }
        public AppException(List<string> messageErrorList):base(messageErrorList)
        {
    

        }
    }
}