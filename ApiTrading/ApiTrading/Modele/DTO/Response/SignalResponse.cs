namespace ApiTrading.Modele.DTO.Response
{
    using System.Collections.Generic;
    using global::Modele;

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