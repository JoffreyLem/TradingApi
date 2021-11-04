using System.Linq;
using System.Threading.Tasks;
using APIhandler;
using Indicator.Indicator;
using Modele;
using Utility;

namespace ScalpingStrategy
{
    public class ScalpingStrategy : Strategy.Strategy
    {
        public ScalpingStrategy(string symbol, Timeframe petitTimeFrame, Timeframe grandTimeframe, ApiHandler handler,
            bool isbacktest) : base(symbol,
            petitTimeFrame, grandTimeframe, handler, isbacktest)
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

                if (PositionInProgress && AllowMultiplePosition)
                    await OpenPosition(signal, sl, tp, 0);
                else if (!PositionInProgress) await OpenPosition(signal, sl, tp, 0);
            }
        }

        public override (bool updatable, double? sl, double? tp) IsPositionUpdatable(Position position)
        {
            var close = History.Last().Close;
            var currentState = SarIndicator.GetState(0, close);
            var positionState = position.TypePosition;

            if (currentState.GetEnumDescription() == positionState.GetEnumDescription())
            {
                var newSl = (double?) SarIndicator.Last().Sar;
                var newTp = 0;
                return (true, newSl, newTp);
            }

            return (false, 0, 0);
        }

        public override bool IsPositionClosable(Position position)
        {
            return false;
        }
    }
}