namespace ApiTrading.StrategyManager
{
    using global::StrategyManager;
    using ScalpingStrategy;

    public enum StrategyList
    {
        [StrategyAttributeType(typeof(ScalpingStrategy), "Strategy basée sur le Sar", "ScalpingStrategy")]
        ScalpingStrategy
    }
}