using System.Collections.Generic;
using Modele;

namespace Indicator
{
    public interface IUpdate
    {
        public void Update(List<Candle> history);
    }
}