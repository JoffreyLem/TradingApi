namespace Strategy
{
    using System;

    public static class StrategyFactory
    {
        public static Strategy GetStrategy(Type type, string symbol, string timeframe)
        {
            return (Strategy)Activator.CreateInstance(type, symbol, timeframe);
        }

        public static Strategy GetStrategy(Type type)
        {
            return (Strategy)Activator.CreateInstance(type);
        }
    }
}