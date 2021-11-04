using System;
using System.Linq;
using System.Threading.Tasks;
using APIhandler;
using Modele;
using Modele.StramingModel;

namespace Strategy.MoneyManagement
{
    public class MoneyManagementHandler
    {
        private double risque;

        public MoneyManagementHandler(double cotation, string symbol, ApiHandler handler, bool isbacktest = false)
        {
            IsBackTest = isbacktest;
            DrawnDown = 20;
            LooseStreakTreshold = 4;
            Risque = 5;
            Handler = handler;
            PositionHandler = new PositionHandler(cotation, symbol, handler, isbacktest);
            PositionHandler.PositionClosedEvent += PositionClosedEvent;
            if (!isbacktest)
            {
                Handler.AddCallBackProfitReceived(StreamingOnProfitRecordReceived);
                Handler.AddCallBackTradeBalanceReceived(StreamingOnBalanceRecordReceived);
                var balancerecord = Handler.GetAccountInfo().Result;
                Balance = balancerecord.Balance;
                Credit = balancerecord.Credit;
                Equity = balancerecord.Equity;
                Margin = balancerecord.Margin;
                // this.MarginFree = balancerecord.Margin_free;
                // this.MarginLevel = balancerecord.Margin_level;
            }
        }

        public ApiHandler Handler { get; set; }
        public PositionHandler PositionHandler { get; set; }

        public EventHandler<MoneyManagementTresholdEventArgs> TresholdEventHandler { get; set; }

        public double LooseStreakTreshold { get; set; }

        public double Risque
        {
            get => risque / 100;
            set => risque = value;
        }

        public double DrawnDown { get; set; }

        public bool IsBackTest { get; set; }

        public double? Balance { get; set; }
        public double? Credit { get; set; }
        public double? Equity { get; set; }
        public double? Margin { get; set; }
        public double? MarginFree { get; set; }
        public double? MarginLevel { get; set; }

        private void PositionClosedEvent(object? sender, Position e)
        {
            if (IsBackTest)
            {
                var closed = e;
                Balance += closed.Profit;
            }
            else
            {
                if (CheckLooseStreakTreshold())
                    TresholdEventHandler.Invoke(this,
                        new MoneyManagementTresholdEventArgs(MoneyManagementTresholdEventArgs
                            .MoneyManagementTresholdEventType.LooseStreak));
                else if (CheckDrawnDownTreshold())
                    TresholdEventHandler.Invoke(this,
                        new MoneyManagementTresholdEventArgs(MoneyManagementTresholdEventArgs
                            .MoneyManagementTresholdEventType.Drowdown));
                else if (CheckProfitFactorTreshold())
                    TresholdEventHandler.Invoke(this,
                        new MoneyManagementTresholdEventArgs(MoneyManagementTresholdEventArgs
                            .MoneyManagementTresholdEventType.Profitfactor));
            }
        }


        public double GetTaillePosition(double entry, double stopLoss, double cotation, TypePosition typePosition)
        {
            double ecart = 0;
            var capital = Equity;
            if (typePosition == TypePosition.Buy)
            {
                ecart = new double();
                ecart = entry - stopLoss;
            }
            else if (typePosition == TypePosition.Sell)
            {
                ecart = stopLoss - entry;
            }

            var ecart2 = ecart / cotation;

            ecart2 = Math.Round(ecart2, 0);
            var posValue = capital * Risque;
            var pipValue = posValue / ecart2;


            #region forefaxgonetest

            var forexagone = posValue / ecart2;

            var forexagone2 = forexagone / 10;

            var t = cotation / entry * forexagone2;

            var t4 = ecart2 * t;

            #endregion

            return Math.Round((double) forexagone2, 2);
        }

        private async void StreamingOnProfitRecordReceived(ProfitRecordStreaming profitrecord)
        {
            var item = PositionHandler.CurrentOrder.FirstOrDefault(x => x.Order == profitrecord.Order);

            if (item is not null)
            {
                item.Profit = profitrecord.Profit;
                if (CheckPerteRisqueTreshold(item.Profit)) await PositionHandler.CloseTrade(item);
            }
        }

        public async Task ProfitHandler(Position position, Tick Tick)

        {
            //var op = TRADE_OPERATION_CODE.GetOperationCode((long?)position.TypePosition);
            //var profit = 0; await XtbHandler.GetProfitCalculation(position.Symbol, position.Volume,
            //    op, position.OpenPrice,
            //    (double)Tick.Bid);
            //position.Profit = profit.Profit;
            //if (CheckPerteRisqueTreshold(profit.Profit))
            //{
            //    await PositionHandler.CloseTrade(position).ConfigureAwait(false);
            //}
        }

        private void StreamingOnBalanceRecordReceived(BalanceRecordStreaming balancerecord)
        {
            Balance = balancerecord.Balance;
            Credit = balancerecord.Credit;
            Equity = balancerecord.Equity;
            Margin = balancerecord.Margin;
            MarginFree = balancerecord.MarginFree;
            MarginLevel = balancerecord.MarginLevel;
        }

        public bool CheckPerteRisqueTreshold(double? profit)
        {
            var posValue = Balance * Risque;
            if (posValue * -1 >= profit) return true;

            return false;
        }

        public bool CheckLooseStreakTreshold()
        {
            var selected = PositionHandler.ClosedOrder.TakeLast((int) LooseStreakTreshold);

            if (selected.All(x => x.Profit < 0)) return true;

            return false;
        }

        public bool CheckDrawnDownTreshold()
        {
            var drawndown = PositionHandler.Result.GetDrawndownMax(Balance);
            var drawDownTheorique = Balance * DrawnDown;

            if (drawndown <= drawDownTheorique) return true;

            return false;
        }

        public bool CheckProfitFactorTreshold()
        {
            var profitfactor = PositionHandler.Result.GetProfitFactor();
            if (profitfactor <= 1) return true;

            return false;
        }
    }
}