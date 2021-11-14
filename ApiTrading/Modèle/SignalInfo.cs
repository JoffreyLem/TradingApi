using Modele;

namespace ApiTrading.Modele
{
    public class SignalInfo
    {
        public Signal Signal { get; set; }
        
        public decimal EntryLevel { get; set; }
        
        public decimal? StopLoss { get; set; }
        
        public decimal? TakeProfit { get; set; }
    }
}