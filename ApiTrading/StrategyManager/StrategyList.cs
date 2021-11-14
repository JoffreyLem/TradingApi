using System;

namespace StrategyManager
{
    public enum StrategyList
    {
        [StrategyAttributeType(typeof(ScalpingStrategy.ScalpingStrategy),"Strategy basée sur le Sar","ScalpingStrategy")]
        ScalpingStrategy,
    }
}