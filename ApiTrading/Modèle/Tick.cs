using System;

namespace Modele
{
    public readonly struct Tick
    {
        public Tick(decimal ask, decimal bid, DateTime date, long askVolume, long bidVolume, double spread)
        {
            Ask = ask;
            Bid = bid;
            Date = date;
            AskVolume = askVolume;
            BidVolume = bidVolume;
            Spread = spread;
        }


        public decimal Ask { get; }
        public long AskVolume { get; }
        public decimal Bid { get; }
        public long BidVolume { get; }
        public DateTime Date { get; }

        public double Spread { get; }

        public override string ToString()
        {
            return $"Date : {Date} " +
                   $"Bid : {Bid} " +
                   $"Ask : {Ask}";
        }
    }
}