using System.ComponentModel;

namespace Modele
{
    public enum Signal
    {
        [Description("Buy")] Buy,

        [Description("Sell")] Sell,

        None
    }
}