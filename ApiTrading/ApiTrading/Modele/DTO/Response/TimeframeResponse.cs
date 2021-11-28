namespace ApiTrading.Modele.DTO.Response
{
    using System.Collections.Generic;

    public class TimeframeResponse
    {
        public TimeframeResponse(List<string> timeframes)
        {
            Timeframes = timeframes;
        }

        public TimeframeResponse(int statusCode, string message, List<string> timeframes)
        {
            Timeframes = timeframes;
        }

        public TimeframeResponse()
        {
        }

        public List<string> Timeframes { get; set; }
    }
}