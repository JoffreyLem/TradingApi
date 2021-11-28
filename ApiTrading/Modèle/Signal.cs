namespace Modele
{
    using System.ComponentModel;

    public enum Signal
    {
        [Description("Buy")] Buy,

        [Description("Sell")] Sell,

        None
    }
}