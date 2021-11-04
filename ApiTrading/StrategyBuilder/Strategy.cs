using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using APIhandler;
using CandleBuilder;
using CandleBuilder.EventArgs;
using Indicator;
using Modele;
using Utility;

namespace Strategy
{
    public abstract class Strategy
    {
        private static object _lock = new();

        public string description;

        public Strategy(string symbol, Timeframe petitTimeframe, Timeframe grandTimeframe, IApiHandler handler,
            bool isBackTest = false)
        {
            ApIhandler = handler;
            IsBackTest = isBackTest;
            Symbol = symbol;
            CanRun = true;
            CandlePairList = new CandlePairList(petitTimeframe, grandTimeframe, symbol, ApIhandler, isBackTest);
            CandlePairList.NewCandle += CandlePairList_NewCandle;
            CandlePairList.NewTick += CandlePairList_NewTick;
        
            if (!IsBackTest)
            {
            }

            AllowMultiplePosition = false;

            var test = Description;
        }

        public bool IsBackTest { get; set; }


        public string Symbol { get; set; }

        public IApiHandler ApIhandler { get; set; }

        public string Description
        {
            get => description;
            set => description = value;
        }
        
        public CandlePairList CandlePairList { get; set; }
        public CandleList History => CandlePairList.List;
        public CandleList HistoryL1 => CandlePairList.List1;
        public List<Tick> TickList => CandlePairList.ListTick;
        public bool AllowMultiplePosition { get; set; }

        public bool CanRun { get; set; }

        public event EventHandler<CandleEventArgs> NewCandle
        {
            add => CandlePairList.NewCandle += value;
            remove => CandlePairList.NewCandle -= value;
        }

        public event EventHandler<TicksEventArgs> NewTick
        {
            add => CandlePairList.NewTick += value;
            remove => CandlePairList.NewTick -= value;
        }
        

        public abstract Task Run();


        protected virtual async void CandlePairList_NewCandle(object sender, CandleEventArgs e)
        {
            if (CanRun && e.Timeframe == CandlePairList.List.TimeFrame)
            {
                await UpdateIndicator();
                await Run();
            }
        }

        protected virtual async void CandlePairList_NewTick(object sender, TicksEventArgs e)
        {
            await UpdateIndicator();
        }


        public async Task UpdateIndicator()
        {
            var truc = GetType().UnderlyingSystemType.GetRuntimeProperties();

            foreach (var info in truc)
                if (info.GetValue(this, null) is IIndicator value)
                {
                    IEnumerable<Candle> history = null;
                    if (value.IndicatorLevel == IndicatorLevel.L1)
                        history = CandlePairList.List.Select(x => x).Reverse().Take(100).Reverse();
                    else if (value.IndicatorLevel == IndicatorLevel.L2)
                        history = CandlePairList.List1.Select(x => x).Reverse().Take(100).Reverse();

                    value.Update(history?.ToList());
                }
        }

    
    }
}