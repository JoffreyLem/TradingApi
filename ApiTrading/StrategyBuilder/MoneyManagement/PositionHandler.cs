using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIhandler;
using Modele;
using Modele.StramingModel;
using Utility;
using XtbLibrairie.codes;

namespace Strategy.MoneyManagement
{
    public class PositionHandler
    {
        public EventHandler<Position> PositionClosedEvent;


        public PositionHandler(double cotation, string symbol, ApiHandler apiHandler, bool isbacktest = false)
        {
            ApiHandler = apiHandler;
            Symbol = symbol;
            Cotation = cotation;
            RejectedOrder = new List<Position>();
            PendingOrder = new List<Position>();
            CurrentOrder = new List<Position>();
            ClosedOrder = new List<Position>();
            IsBackTest = isbacktest;
            ApiHandler.AddCallBackTradeRecordReceived(StreamingOnTradeRecordReceived);
            ApiHandler.AddCallBackTradeStatusRecordReceived(StreamingOnTradeStatusRecordReceived);
            ApiHandler.AddCallBackPriceReceived(StreamingOnTickRecordReceived);
        }

        public ApiHandler ApiHandler { get; set; }
        public double Cotation { get; set; }
        public string Symbol { get; set; }

        public bool IsBackTest { get; set; }

        public Tick LastTick { get; set; }
        public string PositionCommentDescription { get; set; }
        public List<Position> RejectedOrder { get; set; }
        public List<Position> PendingOrder { get; set; }
        public List<Position> CurrentOrder { get; set; }

        public List<Position> ClosedOrder { get; set; }

        public Result Result { get; set; }


        private void StreamingOnTickRecordReceived(TickRecordStreaming tickrecord)
        {
            if (tickrecord.Level == 0 && tickrecord.Symbol == Symbol)
            {
                var bid = (decimal) tickrecord.Bid;
                var ask = (decimal) tickrecord.Ask;
                var date = tickrecord.Timestamp.ConvertToDatetime();
                var bidVolume = (long) tickrecord.BidVolume;
                var askVolume = (long) tickrecord.AskVolume;
                LastTick = new Tick(bid, ask, date, askVolume, bidVolume, (double) tickrecord.SpreadRaw);
            }
        }


        public async Task CloseTrade(Position position)
        {
            if (!IsBackTest)
            {
                var positionInformations = new PositionInformations(0, 0, LastTick.Bid);
                var test = await ApiHandler.ClosePosition(position, positionInformations);
            }
            else
            {
                var op = TRADE_OPERATION_CODE.GetOperationCode((long?) position.TypePosition);
                var profit = ApiHandler.CalculerProfit(position.Symbol, position.Volume,
                    position.TypePosition, position.OpenPrice,
                    position.ClosePrice);
                position.Profit = profit;
                ClosedOrder.Add(position);
                CurrentOrder.Remove(position);
            }

            Result.Positions.Add(position);

            PositionClosedEvent?.Invoke(this, position);
        }

        public async Task UpdatePosition(Position position, double? sl, double? tp)
        {
            sl = Math.Round(sl.GetValueOrDefault(), GetRoundValue());
            tp = Math.Round(tp.GetValueOrDefault(), GetRoundValue());
            if (!IsBackTest)
            {
                var positionInformations = new PositionInformations(sl, tp, LastTick.Bid);
                var test = await ApiHandler.UpdatePosition(position, positionInformations);
            }
            else
            {
                position.StopLoss = sl;
                position.TakeProfit = tp;
            }
        }


        private async Task CorretPositionNumber()
        {
            if (typeof(ApiHandler) == typeof(XtbApiHandler))
            {
                var dataset = await ApiHandler.GetTrades();

                foreach (var datasetTradeRecord in dataset)
                {
                    var selected = CurrentOrder
                        .FirstOrDefault(x => x.Order2 == datasetTradeRecord.Order2);
                    var selected2 = PendingOrder.FirstOrDefault(x => x.Order2 == datasetTradeRecord.Order2);
                    if (selected is not null)
                    {
                        selected.Order = datasetTradeRecord.Order;
                        selected.Order2 = datasetTradeRecord.Order2;
                        selected.PositionID = datasetTradeRecord.PositionID;
                    }

                    if (selected2 is not null)
                    {
                        selected2.Order = datasetTradeRecord.Order;
                        selected2.Order2 = datasetTradeRecord.Order2;
                        selected2.PositionID = datasetTradeRecord.PositionID;
                    }
                }
            }
        }

        private async void StreamingOnTradeStatusRecordReceived(TradeStatusStreaming tradeStatusRecord)
        {
            if (tradeStatusRecord.RequestStatus == RequestStatus.Rejeted)
            {
                var status = StatusPosition.Close;
                var id = tradeStatusRecord.Order;
                var comment = tradeStatusRecord.CustomComment;
                var price = tradeStatusRecord.Price;

                var order = new Position(id, status, comment, price);
                RejectedOrder.Add(order);
            }
        }

        private async void StreamingOnTradeRecordReceived(TradeRecordStreaming traderecord)
        {
            var symbol = traderecord.Symbol;
            var typeposition = traderecord.Cmd;
            var profit = traderecord.Profit;
            var closePrice = traderecord.Close_price;
            var dateClose = traderecord.Close_time.ConvertToDatetime();
            var openPrice = traderecord.Open_price;
            var openDate = traderecord.Open_time.ConvertToDatetime();
            var sl = traderecord.Sl;
            var tp = traderecord.Tp;
            var volume = traderecord.Volume;
            var comment = traderecord.CustomComment;


            if (traderecord?.CustomComment == null)
            {
                if (traderecord?.Type == StatusPosition.Pending)
                {
                    var item = PendingOrder.FirstOrDefault(x => x?.Order2 == traderecord?.Order2);

                    if (item is null)
                    {
                        var pos = new Position(symbol, typeposition, profit, closePrice, dateClose, openPrice, openDate,
                            sl, tp, volume, comment);
                        PendingOrder.Add(pos);
                        await CorretPositionNumber();
                    }
                    else
                    {
                        item?.UpdatePosition(profit, closePrice, dateClose, sl, tp, typeposition);
                    }
                }
                else if (traderecord?.Type == StatusPosition.Open)
                {
                    var item = CurrentOrder.FirstOrDefault(x => x?.Order2 == traderecord?.Order2);

                    if (item is not null)
                    {
                        item?.UpdatePosition(profit, closePrice, dateClose, sl, tp, typeposition);
                    }
                    else
                    {
                        var item2 = PendingOrder.FirstOrDefault(x => x?.Order2 == traderecord?.Order2);


                        CurrentOrder.Add(item2);
                        PendingOrder.Remove(item2);
                    }
                }
                else if (traderecord?.Type == StatusPosition.Close)
                {
                    var item = CurrentOrder.FirstOrDefault(x => x?.Order2 == traderecord?.Order2);

                    if (item is not null)
                    {
                        item?.UpdatePosition(profit, closePrice, dateClose, sl, tp, typeposition);
                        ClosedOrder.Add(item);
                        CurrentOrder.Remove(item);
                    }
                }
            }
        }

        public int GetRoundValue(double? cotation = null)
        {
            var item = cotation ?? Cotation;

            var indice = 0;
            var i2 = (decimal) item;

            do
            {
                i2 = i2 * 10;
                indice++;
            } while (i2 != 1);

            return indice;
        }
    }
}