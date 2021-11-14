using System.Collections.Generic;
using Modele;
using Skender.Stock.Indicators;

namespace Indicator.Indicator
{
    public class SarIndicator : BaseIndicator<ParabolicSarResult>
    {
        public SarIndicator(List<Candle> data, double accelerationstep = 0.02,
            double maxAccelerationFactor = 0.2, IndicatorLevel indicatorLevel = IndicatorLevel.L1) 
        {
            AccelerationStep = new decimal(accelerationstep);
            MaxAccelerationFactor = new decimal(maxAccelerationFactor);
          
        }

        public decimal AccelerationStep { get; set; }
        public decimal MaxAccelerationFactor { get; set; }

        public override bool Buy(int i)
        {
            var data = GetDataSet(i);

            for (var i1 = 0; i1 < data.Count - 1; i1++)
            {
                var actualSar = data[i1];
                var lastSar = data[i1 + 1];

                if (actualSar.IsReversal is true)
                    if (actualSar.Sar < lastSar.Sar)
                        return true;
            }

            return false;
        }

        public override bool Sell(int i)
        {
            var data = GetDataSet(i);
            for (var i1 = 0; i1 < data.Count - 1; i1++)
            {
                var actualSar = data[i1];
                var lastSar = data[i1 + 1];

                if (actualSar.IsReversal is true)
                    if (actualSar.Sar > lastSar.Sar)
                        return true;
            }

            return false;
        }

   
    }
}