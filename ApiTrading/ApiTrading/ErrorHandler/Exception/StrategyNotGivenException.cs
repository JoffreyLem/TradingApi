namespace ApiTrading.Exception
{
    using System.Collections.Generic;

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