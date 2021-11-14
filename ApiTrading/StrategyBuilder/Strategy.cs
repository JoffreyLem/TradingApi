using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CandleBuilder;
using CandleBuilder.EventArgs;
using Indicator;
using Modele;
using Utility;

namespace Strategy
{
    public abstract class Strategy
    {
        public string Description { get; set; }

        public Strategy(string symbol)
        {
            Symbol = symbol;
        }

        public string Symbol { get; set; }

        public abstract Task Run();


     


  

    
    }
}