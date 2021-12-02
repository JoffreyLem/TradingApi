using Modele;

namespace Indicator
{
    public interface IBuySellSignal
    {
        public bool Buy(int i);

        public bool Sell(int i);

        public Signal GetSignal(int i);
    }
}