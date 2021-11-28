namespace ApiTrading.Exception
{
    public class TimeFrameDontExistException : CustomErrorException
    {
        public TimeFrameDontExistException(string? message) : base(message)
        {
        }
    }
}