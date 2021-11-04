using System;
using System.ComponentModel;

namespace Modele
{
    public class Position
    {
        public Position(string symbol, TypePosition typePosition, double? profit, double? closePrice,
            DateTime dateClose, double? openPrice, DateTime dateOpen, double? stopLoss, double? takeProfit,
            double? volume, string comment)
        {
            Symbol = symbol;
            TypePosition = typePosition;

            Profit = profit;
            ClosePrice = closePrice;
            DateClose = dateClose;
            OpenPrice = openPrice;
            DateOpen = dateOpen;
            StopLoss = stopLoss;
            TakeProfit = takeProfit;
            Volume = volume;
            Comment = comment;
            IsClosed = false;
        }


        public Position(string tradeStatusRecord, TypePosition typePosition, decimal? currentBid, double newSl,
            double newTp, string symbol, double lot, string description, DateTime opendate)
        {
            ID2 = tradeStatusRecord;
            TypePosition = typePosition;
            Symbol = symbol;
            OpenPrice = (double?) currentBid;
            StopLoss = newSl;
            TakeProfit = newTp;
            Symbol = symbol;
            Volume = lot;
            Comment = description;
            DateOpen = opendate;
        }

        public Position(long? id, StatusPosition status, string comment, double? price)
        {
            Id = id;
            StatusPosition = status;
            Comment = comment;
            CurrentPrice = price;
        }


        public long? Id { get; set; }
        public string ID2 { get; set; }

        public bool IsClosed { get; set; }

        public string Symbol { get; set; }
        public TypePosition TypePosition { get; set; }
        public double? CurrentPrice { get; set; }
        public double? Spread { get; set; }
        public double? Profit { get; set; }

        public bool Closed { get; set; }

        public StatusPosition StatusPosition { get; set; }

        public double? ClosePrice { get; set; }

        public DateTime DateClose { get; set; }
        public double? OpenPrice { get; set; }
        public DateTime DateOpen { get; set; }

        public double? StopLoss { get; set; }

        public double? TakeProfit { get; set; }

        public double? Volume { get; set; }

        public string Comment { get; set; }

        public string Message { get; set; }


        public void UpdatePosition(double? profit, double? closePrice, DateTime dateClose, double? sl, double? tp,
            TypePosition typeposition)
        {
            Profit = profit;
            ClosePrice = closePrice;
            DateClose = dateClose;
            StopLoss = sl;
            TakeProfit = tp;
            TypePosition = typeposition;
        }

        public override string ToString()
        {
            return
                $"Type position:{TypePosition} Date Open:{DateOpen} Prix Open:{OpenPrice} Date Close:{DateClose} Prix Close:{ClosePrice} SL:{StopLoss} TP:{TakeProfit} \n";
        }


        #region XTB Specifique

        public long? Order { get; set; }
        public long? Order2 { get; set; }

        public long? PositionID { get; set; }

        #endregion
    }

    public enum StatusPosition
    {
        Open,
        Pending,
        Close
    }

    public enum TypePosition
    {
        [Description("Buy")] Buy = 0,
        [Description("Sell")] Sell = 1,
        BuyLimit = 2,
        SellLimit = 3,
        BuyStop = 4,
        SellStop = 5,
        Balance = 6,
        Credit = 7
    }
}