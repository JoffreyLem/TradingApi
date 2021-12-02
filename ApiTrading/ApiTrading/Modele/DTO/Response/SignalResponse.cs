using System.Collections.Generic;
using Modele;

namespace ApiTrading.Modele.DTO.Response
{
    public class SignalResponse
    {
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

        public List<SignalInfoStrategy> Signals { get; set; }
    }
}