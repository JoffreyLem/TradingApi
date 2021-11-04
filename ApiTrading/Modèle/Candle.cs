using System;
using System.Collections.Generic;
using Skender.Stock.Indicators;

namespace Modele
{
    public class Candle : IQuote
    {
        private decimal close;

        public Candle()
        {
        }

        public Candle(Candle candle)
        {
            Date = candle.Date;

            Open = candle.Open;
            High = candle.High;
            Low = candle.Low;
            Close = candle.close;
            Volume = candle.Volume;
            Type = Close > Open ? CandleType.buy : CandleType.sell;
            AskVolume = 0;
            BidVolume = 0;
            ListTick = new List<Tick>();
        }


        public Candle(decimal open, decimal high, decimal low, decimal close, DateTime date, decimal volume)
        {
            Date = date;

            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            Type = close > open ? CandleType.buy : CandleType.sell;
            AskVolume = 0;
            BidVolume = 0;
            ListTick = new List<Tick>();
        }

        public Candle(double? open, double? high, double? low, double? close, DateTime date, double? volume,
            double? symbolTickSize)
        {
            var rationConverted = (decimal) symbolTickSize;
            Date = date;
            Open = (decimal) open * rationConverted;
            High = Open + (decimal) high * rationConverted;
            Low = Open + (decimal) low * rationConverted;
            Close = Open + (decimal) close * rationConverted;
            Volume = (decimal) volume;
            Type = Close > Open ? CandleType.buy : CandleType.sell;

            ListTick = new List<Tick>();
        }


        public long BidVolume { get; set; }
        public long AskVolume { get; set; }


        public CandleType Type { get; set; }


        public decimal Body
        {
            get
            {
                if (Type == CandleType.buy)
                    return close - Open;
                return Open - close;
            }
        }

        public decimal TopMeche
        {
            get
            {
                if (Type == CandleType.buy)
                    return High - close;
                return High - Open;
            }
        }

        public decimal BotMeche
        {
            get
            {
                if (Type == CandleType.buy)
                    return Open - Low;
                return Close - Low;
            }
        }

        public decimal MedianPoint => Body / 2;

        public List<Tick> ListTick { get; set; }

        public DateTime Date { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close
        {
            get => close;
            set
            {
                close = value;
                if (value >= High) High = value;

                if (value <= Low) Low = value;
            }
        }

        public decimal Volume { get; set; }

        public override string ToString()
        {
            return "[NEW CANDLE] " + $"Date:{Date} " + $"Open:{Open} " + $"High:{High} " + $"Low:{Low} " +
                   $"Close:{Close}";
        }
    }

    public enum CandleType
    {
        buy,
        sell
    }
}