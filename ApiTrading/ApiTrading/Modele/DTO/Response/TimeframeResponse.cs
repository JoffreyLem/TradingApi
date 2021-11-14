using System.Collections.Generic;

namespace ApiTrading.Modele.DTO.Response
{
    public class TimeframeResponse : ResponseModel
    {
        public List<string> Timeframes { get; set; }

        public TimeframeResponse(List<string> timeframes)
        {
            Timeframes = timeframes;
        }

        public TimeframeResponse(int statusCode, string message, List<string> timeframes) : base(statusCode, message)
        {
            Timeframes = timeframes;
        }

        public TimeframeResponse(int statusCode, string message): base(statusCode, message)
        {
          
        }
    }
}