namespace ScalpingStrategy
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Indicator.Indicator;
    using Modele;
    using Strategy;

    public class ScalpingStrategy : Strategy
    {
        public ScalpingStrategy(string symbol, string timeframe) : base(symbol)
        {
            Timeframe = timeframe;
            Symbol = symbol;

            CciIndicator = new CciIndicator(null);
            SarIndicator = new SarIndicator(null);
            FastSarIndicator = new SarIndicator(null, 0.08);
            Description = "Scalping strategy TEST";
        }

        public string Symbol { get; }

        public string Timeframe { get; set; }


        public CciIndicator CciIndicator { get; set; }
        public SarIndicator SarIndicator { get; set; }
        public SarIndicator FastSarIndicator { get; set; }


        public override async Task<List<SignalInfoStrategy>> Run(int? index)
        {
            var signalInfos = new List<SignalInfoStrategy>();
            index = index ?? 0;
            for (var i = index.Value; i < History.Count; i++)
                if (i > 1)
                {
                    var cciSignal = CciIndicator.GetSignal(3);
                    var sarSignal = SarIndicator.GetSignal(1);
                    var globalSignal = cciSignal == sarSignal;

                    if (globalSignal)
                    {
                        var signalInfo = new SignalInfoStrategy(Timeframe, Symbol);
                        signalInfo.Signal = sarSignal;
                        signalInfo.EntryLevel = History[i].Close;
                        signalInfo.DateTime = History[i].Date;
                        signalInfos.Add(signalInfo);
                    }
                }

            return signalInfos;
        }
    }
}