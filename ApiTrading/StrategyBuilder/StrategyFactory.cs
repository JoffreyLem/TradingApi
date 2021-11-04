using System;
using Modele;

namespace Strategy
{
    public static class StrategyFactory
    {
        public static Strategy GetStrategy(Type type, string symbol, Timeframe petit, Timeframe grand, bool backtest)
        {
            return (Strategy) Activator.CreateInstance(type, symbol, petit, grand, true);
        }
    }
}