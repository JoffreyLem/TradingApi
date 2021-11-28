namespace Indicator.Indicator
{
    using System.Collections.Generic;
    using Modele;
    using Skender.Stock.Indicators;

    public class CciIndicator : BaseIndicator<CciResult>
    {
        public CciIndicator(List<Candle> history, int lookbackPeriod = 20,
            IndicatorLevel indicatorLevel = IndicatorLevel.L1) : base(lookbackPeriod)
        {
        }

        public override bool Buy(int i)
        {
            var data = GetDataSet(i);

            for (var i1 = 0; i1 < data.Count - 1; i1++)
            {
                var actualCci = data[i1];
                var lastCci = data[i1 + 1];
                var test = lastCci.Cci < -100;
                if (lastCci.Cci < -100 && actualCci.Cci > -100) return true;
            }

            return false;
        }

        public override bool Sell(int i)
        {
            var data = GetDataSet(i);

            for (var i1 = 0; i1 < data.Count - 1; i1++)
            {
                var actualCci = data[i1];
                var lastCci = data[i1 + 1];

                if (lastCci.Cci > 100 && actualCci.Cci < 100) return true;
            }

            return false;
        }

        public sealed override void Update(List<Candle> history)
        {
            if (history.Count > LookbackPeriod)
            {
                var data = Indicator.GetCci(history, LookbackPeriod);
                base.Update(data);
            }
        }
    }
}