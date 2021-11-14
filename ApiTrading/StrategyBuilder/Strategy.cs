using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApiTrading.Modele;
using CandleBuilder;
using CandleBuilder.EventArgs;
using Indicator;
using Modele;
using Utility;

namespace Strategy
{
    public abstract class Strategy
    {
        private List<Candle> _history;
        public string Description { get; set; }

        public Strategy(string symbol)
        {
            Symbol = symbol;
        }

        public string Symbol { get; set; }

        public List<Candle> History
        {
            get => _history;
            set
            {
                _history = value;
                UpdateIndicator();
            }
        }

        public abstract Task<List<SignalInfo>> Run();


     
        public async Task UpdateIndicator()
        {
            var truc = GetType().UnderlyingSystemType.GetRuntimeProperties();

            foreach (var info in truc)
                if (info.GetValue(this, null) is IIndicator value)
                {

                    value.Update(History?.ToList());
                }
        }

  

    
    }
}