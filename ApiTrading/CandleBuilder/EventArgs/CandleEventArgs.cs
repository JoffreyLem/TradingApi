using Modele;

namespace CandleBuilder.EventArgs
{
    public class CandleEventArgs : System.EventArgs
    {
        public CandleEventArgs(Candle candle, string timeFrame)
        {
            Candle = candle;
            Timeframe = timeFrame;
        }

        public Candle Candle { get; set; }
        public string Timeframe { get; set; }
    }
}