namespace Indicator
{
    using System.Collections.Generic;
    using Modele;

    public interface IUpdate
    {
        public void Update(List<Candle> history);
    }
}