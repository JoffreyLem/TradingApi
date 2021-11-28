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
        
        public string Timeframe { get; set; }

        
  

        public ScalpingStrategy(string symbol,string timeframe) : base(symbol)
        {
            Timeframe = timeframe;
            Symbol = symbol;
        
            CciIndicator = new CciIndicator(null);
            SarIndicator = new SarIndicator(null);
            FastSarIndicator = new SarIndicator(null, 0.08);
            Description = "Scalping strategy TEST";
        }
        
     



        public CciIndicator CciIndicator { get; set; }
        public SarIndicator SarIndicator { get; set; }
        public SarIndicator FastSarIndicator { get; set; }

  

        public override async Task<List<SignalInfoStrategy>> Run()
        {
            List<SignalInfoStrategy> signalInfos = new List<SignalInfoStrategy>();
            
            for (var i = 0; i < History.Count; i++)
            {
                if (i > 1)
                {
                    var cciSignal = CciIndicator.GetSignal(3);
                    var sarSignal = SarIndicator.GetSignal(1);
                    var globalSignal = cciSignal == sarSignal;

                    if (globalSignal)
                    {
                        var signalInfo = new SignalInfoStrategy(Timeframe,Symbol);
                        signalInfo.Signal = sarSignal;
                        signalInfo.EntryLevel = History[i].Close;
                        signalInfo.DateTime = History[i].Date;
                        signalInfos.Add(signalInfo);
                    }
                }
            }
            return signalInfos;
        }

        
    }
}