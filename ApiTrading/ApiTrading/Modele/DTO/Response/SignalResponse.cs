using System.Collections.Generic;

namespace ApiTrading.Modele.DTO.Response
{
    using global::Modele;

    public class SignalResponse 
    {
        public List<SignalInfoStrategy> Signals { get; set; }


        public SignalResponse()
        {
            
        }
     

        public SignalResponse(List<SignalInfoStrategy> signals) 
        {
            Signals = signals;
        }

        public SignalResponse(int statusCode, string message)
        {
           
        }
    }
}