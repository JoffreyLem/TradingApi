using System.Linq;
using System.Threading.Tasks;
using Indicator.Indicator;
using Modele;
using Utility;

namespace ScalpingStrategy
{
    public class ScalpingStrategy : Strategy.Strategy
    {
        public ScalpingStrategy(string symbol, Timeframe petitTimeFrame, Timeframe grandTimeframe, 
            bool isbacktest) : base(symbol,
            petitTimeFrame, grandTimeframe,  isbacktest)
        {
            CciIndicator = new CciIndicator(History);
            SarIndicator = new SarIndicator(History);
            SarIndicatorL1 = new SarIndicator(HistoryL1);
            FastSarIndicator = new SarIndicator(History, 0.08);
            FastSarIndicatorL1 = new SarIndicator(HistoryL1, 0.08);
            Description = "Scalping strategy TEST";
        }

        public CciIndicator CciIndicator { get; set; }
        public SarIndicator SarIndicator { get; set; }

        public SarIndicator SarIndicatorL1 { get; set; }
        public SarIndicator FastSarIndicator { get; set; }

        public SarIndicator FastSarIndicatorL1 { get; set; }

        public override async Task Run()
        {
            var close = History.Last().Close;
            var refSignal = SarIndicatorL1.GetState(0, close);
            // var cciSignal = CciIndicator.GetSignal(3);
            var sarSignal = SarIndicator.GetSignal(1);
            var globalSignal = /*cciSignal ==*/ sarSignal;
            if (sarSignal == refSignal && sarSignal != Signal.None)
            {
                var signal = (TypePosition) sarSignal.GetTypePositionBySignal();
                var sl = SarIndicator.Last().Sar;
                var tp = 0;

            }
        }

        
    }
}