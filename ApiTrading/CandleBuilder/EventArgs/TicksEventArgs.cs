using Modele;

namespace CandleBuilder.EventArgs
{
    public class TicksEventArgs : System.EventArgs
    {
        public TicksEventArgs(Tick tick)
        {
            Tick = tick;
        }

        public Tick Tick { get; set; }
    }
}