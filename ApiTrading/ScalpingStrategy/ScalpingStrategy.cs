using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indicator.Indicator;
using Modele;
using Utility;

namespace ScalpingStrategy
{
    public class ScalpingStrategy : Strategy.Strategy
    {
        public string Symbol { get; private set; }
        public List<Candle> History { get;  set; }

        public ScalpingStrategy(string symbol, List<Candle> history) : base(symbol)
        {
            Symbol = symbol;
            this.History = history;
            CciIndicator = new CciIndicator(history);
            SarIndicator = new SarIndicator(history);
            FastSarIndicator = new SarIndicator(history, 0.08);
            Description = "Scalping strategy TEST";
        }



        public CciIndicator CciIndicator { get; set; }
        public SarIndicator SarIndicator { get; set; }
        public SarIndicator FastSarIndicator { get; set; }

  

        public override async Task Run()
        {
            // var close = History.Last().Close;
            //
            // // var cciSignal = CciIndicator.GetSignal(3);
            // var sarSignal = SarIndicator.GetSignal(1);
            // var globalSignal = /*cciSignal ==*/ sarSignal;
            // if (sarSignal == refSignal && sarSignal != Signal.None)
            // {
            //     var signal = (TypePosition) sarSignal.GetTypePositionBySignal();
            //     var sl = SarIndicator.Last().Sar;
            //     var tp = 0;
            //
            // }
        }

        
    }
}