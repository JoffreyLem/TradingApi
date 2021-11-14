using StrategyManager;

namespace ApiTrading.StrategyManager
{
    public enum StrategyList
    {
        [StrategyAttributeType(typeof(ScalpingStrategy.ScalpingStrategy),"Strategy basée sur le Sar","ScalpingStrategy")]
        ScalpingStrategy,
    }
}