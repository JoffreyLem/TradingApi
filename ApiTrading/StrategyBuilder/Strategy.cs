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
using Strategy.MoneyManagement;
using Utility;

namespace Strategy
{
    public abstract class Strategy
    {
        private static object _lock = new();

        public string description;

        public Strategy(string symbol, Timeframe petitTimeframe, Timeframe grandTimeframe, ApiHandler handler,
            bool isBackTest = false)
        {
            ApIhandler = handler;
            IsBackTest = isBackTest;
            Symbol = symbol;
            CanRun = true;
            CandlePairList = new CandlePairList(petitTimeframe, grandTimeframe, symbol, ApIhandler, isBackTest);
            CandlePairList.NewCandle += CandlePairList_NewCandle;
            CandlePairList.NewTick += CandlePairList_NewTick;
            MoneyManagementHandler =
                new MoneyManagementHandler(CandlePairList.TickSize.Value, symbol, ApIhandler, isBackTest);
            MoneyManagementHandler.TresholdEventHandler += TresholdEventHandler;
            if (!IsBackTest)
            {
            }

            AllowMultiplePosition = false;

            var test = Description;
        }

        public bool IsBackTest { get; set; }


        public string Symbol { get; set; }

        public ApiHandler ApIhandler { get; set; }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public bool PositionInProgress => MoneyManagementHandler?.PositionHandler.CurrentOrder.Count > 0;
        public CandlePairList CandlePairList { get; set; }
        public CandleList History => CandlePairList.List;
        public CandleList HistoryL1 => CandlePairList.List1;
        public List<Tick> TickList => CandlePairList.ListTick;
        public MoneyManagementHandler MoneyManagementHandler { get; set; }
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


        private void TresholdEventHandler(object? sender, MoneyManagementTresholdEventArgs e)
        {
            MoneyManagementHandler.PositionHandler.Result.PrintResult(MoneyManagementHandler.Balance);
            Console.WriteLine($"Reason EXIT : {e.EventType.GetEnumDescription()}");
            Environment.Exit(1);
        }


        public abstract Task Run();


        protected virtual async void CandlePairList_NewCandle(object sender, CandleEventArgs e)
        {
            if (CanRun && e.Timeframe == CandlePairList.List.TimeFrame) await Run();
        }

        protected virtual async void CandlePairList_NewTick(object sender, TicksEventArgs e)
        {
            await UpdateIndicator();
            await UpdatePositions();
            await CloseTrades();
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

        public async Task OpenPosition(TypePosition cmd,
            decimal? sl,
            decimal? tp,
            long? expiration)
        {
            var cotation = CandlePairList?.SymbolData?.TickSize;
            var typePosition = cmd;


            var currentBid = History.ListTick.Last().Bid;
            var currentDate = History.ListTick.Last().Date;
            // var lot = MoneyManagementHandler.GetTaillePosition((double)currentBid, (double)sl, (double)cotation, typePosition);
            var lot = 0.02;
            var ind = MoneyManagementHandler.PositionHandler.GetRoundValue();
            var newSl = (double) Math.Round(sl.GetValueOrDefault(), ind);
            var newTp = (double) Math.Round(tp.GetValueOrDefault(), ind);

            if (!IsBackTest)
            {
                var orderNumber =
                    await ApIhandler.OpenPosition(cmd, (double) currentBid, newSl, newTp, Symbol, lot, null, expiration,
                        Description);
            }
            else
            {
                var position = new Position(Guid.NewGuid().ToString("N"), typePosition, currentBid, newSl, newTp,
                    Symbol, lot, description, currentDate);
                MoneyManagementHandler.PositionHandler.CurrentOrder.Add(position);
            }
        }

        public async Task UpdatePositions()
        {
            if (PositionInProgress)
                for (var i = MoneyManagementHandler.PositionHandler.CurrentOrder.Count - 1; i >= 0; i--)
                {
                    var position = MoneyManagementHandler.PositionHandler.CurrentOrder[i];
                    var last = CandlePairList.List.ListTick.Last();
                    if (IsBackTest)
                    {
                        if (CheckIfPositionShouldBeClosedBacktest(position, last))
                        {
                            await MoneyManagementHandler.PositionHandler.CloseTrade(position);
                            return;
                        }

                        await MoneyManagementHandler.ProfitHandler(position, CandlePairList.List.ListTick.Last());
                    }

                    var data = IsPositionUpdatable(position);
                    if (data.updatable)
                    {
                        var sl = data.sl;
                        var tp = data.tp;

                        if (sl is null || sl == 0 || sl == position.StopLoss) sl = position.StopLoss;
                        if (tp is null || tp == 0 || tp == position.TakeProfit) tp = position.TakeProfit;

                        await MoneyManagementHandler.PositionHandler.UpdatePosition(position, sl, tp);
                    }
                }
        }

        public bool CheckIfPositionShouldBeClosedBacktest(Position position, Tick currentPrice)
        {
            var price = (double) currentPrice.Bid;
            if (position.TypePosition == TypePosition.Buy)
            {
                if (position.StopLoss is not null && position.StopLoss != 0 && price < position.StopLoss)
                {
                    position.ClosePrice = position.StopLoss;
                    position.DateClose = currentPrice.Date;
                    return true;
                }

                if (position.TakeProfit is not null && position.TakeProfit != 0 && price > position.TakeProfit)
                {
                    position.ClosePrice = position.TakeProfit;
                    position.DateClose = currentPrice.Date;
                    return true;
                }

                return false;
            }

            if (position.TypePosition == TypePosition.Sell)
            {
                if (position.StopLoss is not null && position.StopLoss != 0 && price > position.StopLoss)
                {
                    position.ClosePrice = position.StopLoss;
                    position.DateClose = currentPrice.Date;
                    return true;
                }

                if (position.TakeProfit is not null && position.TakeProfit != 0 && price < position.TakeProfit)
                {
                    position.ClosePrice = position.TakeProfit;
                    position.DateClose = currentPrice.Date;
                    return true;
                }

                return false;
            }

            return false;
        }


        public abstract (bool updatable, double? sl, double? tp) IsPositionUpdatable(Position position);


        public virtual async Task CloseTrades()
        {
            if (PositionInProgress)
                for (var i = MoneyManagementHandler.PositionHandler.CurrentOrder.Count - 1; i >= 0; i--)
                {
                    var position = MoneyManagementHandler.PositionHandler.CurrentOrder[i];
                    if (IsPositionClosable(position)) await MoneyManagementHandler.PositionHandler.CloseTrade(position);
                }
        }

        public abstract bool IsPositionClosable(Position position);


        public async void BacktestEnd()
        {
            if (PositionInProgress)
            {
                var lastPrice = CandlePairList.List.ListTick.Last();
                for (var i = MoneyManagementHandler.PositionHandler.CurrentOrder.Count - 1; i >= 0; i--)
                {
                    var position = MoneyManagementHandler.PositionHandler.CurrentOrder[i];
                    position.ClosePrice = lastPrice.Bid as double?;
                    position.DateClose = lastPrice.Date;
                    await MoneyManagementHandler.PositionHandler.CloseTrade(position);
                }
            }
        }
    }
}