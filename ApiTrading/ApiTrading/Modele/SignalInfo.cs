using Modele;

namespace ApiTrading.Modele
{
    public class SignalInfo
    {
        public SignalInfo(SignalInfoStrategy signalInfo)
        {
            Signal = signalInfo.Signal;
            EntryLevel = signalInfo.EntryLevel;
            StopLoss = signalInfo.StopLoss;
            TakeProfit = signalInfo.TakeProfit;

        }

        public Signal Signal { get; set; }
        
        public decimal EntryLevel { get; set; }
        
        public decimal? StopLoss { get; set; }
        
        public decimal? TakeProfit { get; set; }
    }
}