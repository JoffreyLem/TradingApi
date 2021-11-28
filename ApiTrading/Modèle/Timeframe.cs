namespace Modele
{
    using System.ComponentModel;

    public enum Timeframe
    {
        [Description("1m")] OneMinute,
        [Description("5m")] FiveMinutes,
        [Description("15m")] FifteenMinutes,
        [Description("30m")] ThirtyMinutes,
        [Description("1h")] OneHour,
        [Description("4h")] FourHour,
        [Description("1d")] Daily,
        [Description("1w")] Weekly,
        [Description("1mn")] Monthly
    }
}