using System.Collections.Generic;

namespace ApiTrading.Modele.DTO.Response
{
    public class SignalResponse : ResponseModel
    {
        public List<SignalInfo> Signals { get; set; }

        public SignalResponse(List<SignalInfo> signals)
        {
            Signals = signals;
        }

        public SignalResponse(int statusCode, string message, List<SignalInfo> signals) : base(statusCode, message)
        {
            Signals = signals;
        }

        public SignalResponse(int statusCode, string message): base(statusCode, message)
        {
           
        }
    }
}