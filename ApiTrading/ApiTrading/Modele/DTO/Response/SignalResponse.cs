using System.Collections.Generic;

namespace ApiTrading.Modele.DTO.Response
{
    public class SignalResponse 
    {
        public List<SignalInfo> Signals { get; set; }

     

        public SignalResponse(List<SignalInfo> signals) 
        {
            Signals = signals;
        }

        public SignalResponse(int statusCode, string message)
        {
           
        }
    }
}