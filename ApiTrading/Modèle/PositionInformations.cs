namespace Modele
{
    public class PositionInformations
    {
        public PositionInformations(double? sl, double? tp, decimal? price)
        {
            Sl = sl;
            Tp = tp;
            Price = price;
        }

        public double? Sl { get; set; }
        public double? Tp { get; set; }
        public decimal? Price { get; set; }
    }
}