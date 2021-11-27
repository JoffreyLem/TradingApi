using System;
using System.Collections.Generic;
using Modele;

namespace Strategy
{
    public static class StrategyFactory
    {
        public static Strategy GetStrategy(Type type, string symbol,string timeframe, List<Candle> history)
        {
            return (Strategy) Activator.CreateInstance(type, symbol,timeframe, history);
        }
        
        public static Strategy GetStrategy(Type type)
        {
            return (Strategy) Activator.CreateInstance(type);
        }
    }
}