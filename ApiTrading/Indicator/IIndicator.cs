using Modele;
using Utility;

namespace Indicator
{
    public interface IIndicator :  IBuySellSignal
    {
        public int LookbackPeriod { get; set; }

        public IndicatorLevel IndicatorLevel { get; set; }

        public Log Log { get; set; }

        public Signal GetState(int i, decimal? close = null);
    }
}