using System;
using System.Collections.Generic;
using Modele;

namespace Strategy
{
    public static class StrategyFactory
    {
        public static Strategy GetStrategy(Type type, string symbol, List<Candle> history)
        {
            return (Strategy) Activator.CreateInstance(type, symbol, history);
        }
        
        public static Strategy GetStrategy(Type type)
        {
            return (Strategy) Activator.CreateInstance(type);
        }
    }
}