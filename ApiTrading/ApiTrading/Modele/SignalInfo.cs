using Modele;

namespace ApiTrading.Modele
{
    using System;

    public class SignalInfo
    {
        public SignalInfo(SignalInfoStrategy signalInfo)
        {
            Signal = signalInfo.Signal;
            EntryLevel = signalInfo.EntryLevel;
            StopLoss = signalInfo.StopLoss;
            TakeProfit = signalInfo.TakeProfit;
            DateTime = signalInfo.DateTime;

        }

        public Signal Signal { get; set; }
        
        public DateTime DateTime { get; set; }
        
        public decimal EntryLevel { get; set; }
        
        public decimal? StopLoss { get; set; }
        
        public decimal? TakeProfit { get; set; }
    }
}