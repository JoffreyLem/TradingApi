namespace Modele
{
    public class SignalInfoStrategy
    {
        public Signal Signal { get; set; }
        
        public decimal EntryLevel { get; set; }
        
        public decimal? StopLoss { get; set; }
        
        public decimal? TakeProfit { get; set; }
    }
}