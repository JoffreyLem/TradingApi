using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CandleBuilder.EventArgs;
using Modele;
using Modele.StramingModel;
using Utility;

namespace CandleBuilder
{
    public class CandlePairList
    {
        private List<Tick> listTick;

        public CandlePairList()
        {
        }

        public CandlePairList(Timeframe petitTimeFrame, Timeframe grandTimeFrame, string symbol, 
            bool isbacktest = false, bool useHistory = true)
        {
            ListTick = new List<Tick>();
            List = new CandleList(petitTimeFrame, symbol, TickSize,  useHistory);
            List1 = new CandleList(grandTimeFrame, symbol, TickSize,  useHistory);
            Symbol = symbol;
            List.NewCandle += List_NewCandle;
            List1.NewCandle += List_NewCandle;
            List.SetTickList(ref listTick);
            List1.SetTickList(ref listTick);
        }




        public string Symbol { get; set; }
        public CandleList List { get; set; }
        public CandleList List1 { get; set; }

        public List<Tick> ListTick
        {
            get => listTick;
            set => listTick = value;
        }

        public SymbolInformations SymbolData { get; set; }

        public double? TickSize => SymbolData?.TickSize;

        public event EventHandler<CandleEventArgs> NewCandle;

        public event EventHandler<TicksEventArgs> NewTick;

        private void List_NewCandle(object sender, CandleEventArgs e)
        {
            NewCandle?.Invoke(sender, e);
        }

        private void StreamingOnCandleRecordReceived(CandleRecordStreaming candlerecord)
        {
            if (List.TimeFrame == Timeframe.OneMinute.GetEnumDescription())
                List.StreamingOnCandleRecordReceived(candlerecord);
        }

        private void StreamingOnTickRecordReceived(TickRecordStreaming tickrecord)
        {
            if (tickrecord.Level == 0 && tickrecord.Symbol == Symbol)
            {
                var tick = new Tick((decimal) tickrecord.Ask, (decimal) tickrecord.Bid,
                    tickrecord.Timestamp.ConvertToDatetime(), (long) tickrecord.AskVolume, (long) tickrecord.BidVolume,
                    (double) tickrecord.SpreadRaw);
                List.StreamingOnTickRecordReceived(tick);
                List1.StreamingOnTickRecordReceived(tick);
                NewTick?.Invoke(this, new TicksEventArgs(tick));
            }
        }


        public async Task NewTickBackTest(Tick tick)
        {
            List.StreamingOnTickRecordReceived(tick);
            List1.StreamingOnTickRecordReceived(tick);
            var ticksEvent = new TicksEventArgs(tick);
            NewTick?.Invoke(this, ticksEvent);
        }
    }
}