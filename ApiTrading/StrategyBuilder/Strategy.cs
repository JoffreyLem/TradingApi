namespace Strategy
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Indicator;
    using Modele;

    public abstract class Strategy
    {
        private List<Candle> _history;

        public Strategy(string symbol)
        {
            Symbol = symbol;
        }

        protected Strategy()
        {
        }

        public string Description { get; set; }

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

        public abstract Task<List<SignalInfoStrategy>> Run(int? index);


        public async Task UpdateIndicator()
        {
            var truc = GetType().UnderlyingSystemType.GetRuntimeProperties();

            foreach (var info in truc)
                if (info.GetValue(this, null) is IIndicator value)
                    value.Update(History?.ToList());
        }
    }
}