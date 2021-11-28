namespace Modele
{
    using System;
    using Skender.Stock.Indicators;

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
        }


        public Candle(decimal open, decimal high, decimal low, decimal close, DateTime date, decimal volume)
        {
            Date = date;

            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public Candle(double? open, double? high, double? low, double? close, DateTime date, double? volume,
            double? symbolTickSize)
        {
            var rationConverted = (decimal)symbolTickSize;
            Date = date;
            Open = (decimal)open * rationConverted;
            High = Open + (decimal)high * rationConverted;
            Low = Open + (decimal)low * rationConverted;
            Close = Open + (decimal)close * rationConverted;
            Volume = (decimal)volume;
        }


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