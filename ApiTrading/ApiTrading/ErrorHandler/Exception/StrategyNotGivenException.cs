using System.Collections.Generic;

namespace ApiTrading.Exception
{
    public class StrategyNotGivenException : CustomErrorException
    {
        public StrategyNotGivenException(List<string> errorsMessages) : base(errorsMessages)
        {
        }

        public StrategyNotGivenException(string? message) : base(message)
        {
        }
    }
}