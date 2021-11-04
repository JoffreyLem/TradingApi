using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIhandler;
using CandleBuilder.EventArgs;
using Modele;
using Modele.StramingModel;
using Utility;

namespace CandleBuilder
{
    public class CandleList : List<Candle>
    {
        public CandleList()
        {
        }

        public CandleList(Timeframe tf, string symbol, double? symbolTickSyze, ApiHandler handler,
            bool useHistory = false)
        {
            Handler = handler;
            IsProcessRunning = true;
            if (useHistory)
            {
                var data = Handler.GetAllChart(symbol, tf.GetEnumDescription(), symbolTickSyze).Result;
                AddRange(data);
            }

            ListTick = new List<Tick>();
            TimeFrame = tf.GetEnumDescription();
            Symbol = symbol;
            ProcessCanRun = false;
            SymbolTickSyze = symbolTickSyze;
        }

        public ApiHandler Handler { get; set; }
        public bool ProcessCanRun { get; set; }
        public string TimeFrame { get; set; }
        public string Symbol { get; set; }
        public double? SymbolTickSyze { get; set; }
        public List<Tick> ListTick { get; set; }

        private bool IsProcessRunning { get; }
        private DateTime? InternalNextDate { get; set; }

        public void SetTickList(ref List<Tick> list)
        {
            ListTick = list;
        }

        public event EventHandler<CandleEventArgs> NewCandle;


        public virtual void StreamingOnCandleRecordReceived(CandleRecordStreaming candlerecord)
        {
        }

        public virtual async void StreamingOnTickRecordReceived(Tick tickrecord)
        {
            if (!ProcessCanRun)
            {
                var test = CheckForDate(tickrecord.Date);
                if (!test) return;
            }

            ListTick.Add(tickrecord);
            if (ListTick.Count > 1)
            {
                var lastTick = ListTick.Last();
                var lastTick2 = ListTick[ListTick.Count() - 2];
                var currentPrice = lastTick.Bid;
                var currentAskVolume = lastTick.AskVolume;
                var currentBidVolume = lastTick.BidVolume;
                var refIndice = GetCandleIndiceBuilder();
                var tickDate = tickrecord.Date;

                if (this.LastOrDefault() is null)
                {
                    var date1 = tickDate;
                    var date2 = new DateTime(date1.Year, date1.Month, date1.Day, date1.Hour, date1.Minute, 0);
                    var candle = new Candle(currentPrice, currentPrice, currentPrice, currentPrice, date2, 0);
                    candle.AskVolume += tickrecord.AskVolume;
                    candle.BidVolume += tickrecord.BidVolume;
                    Add(candle);
                    return;
                }

                var last = this.LastOrDefault();
                var nextDate = last?.Date.AddMinutes(refIndice);
                last.ListTick.Add(tickrecord);
                if (tickDate >= nextDate)
                {
                    var date = (DateTime) nextDate;
                    var candle = new Candle(currentPrice, currentPrice, currentPrice, currentPrice,
                        date, 0);
                    candle.AskVolume += currentAskVolume;
                    candle.BidVolume += currentBidVolume;
                    Add(candle);

                    if (NewCandle != null)
                    {
                        var candleEvent = new CandleEventArgs(last, TimeFrame);
                        NewCandle(this, candleEvent);
                    }
                }
                else
                {
                    last.Close = lastTick.Bid;
                    last.Type = last.Close > last.Open ? CandleType.buy : CandleType.sell;
                    last.AskVolume += lastTick.AskVolume;
                    last.BidVolume += lastTick.BidVolume;
                }
            }
        }

        private async Task CorrectHistory(long? start, long? end)
        {
            var test = start.ConvertToDatetime();
            var test2 = start.ConvertToDatetime().AddMinutes(+1).ConvertToUnixTime();
            var data = await Handler.GetPartialChart(Symbol, TimeFrame, SymbolTickSyze, start, test2);
            foreach (var candle in data)
            {
                Add(candle);
                if (NewCandle != null)
                {
                    var candleEvent = new CandleEventArgs(candle, TimeFrame);
                    NewCandle(this, candleEvent);
                }
            }
        }

        private bool CheckForDate(DateTime date)
        {
            switch (TimeFrame)
            {
                case "1m":
                    var m1 = Enumerable.Range(0, 59).ToArray();
                    if (date.Second == 00)
                    {
                        ProcessCanRun = true;
                        return true;
                    }
                    else
                    {
                        return HandleRunProcess(date, 1);
                    }

                    return false;


                case "5m":
                    int[] m5 = {00, 05, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55};
                    if (m5.Contains(date.Minute) && date.Second == 00)
                    {
                        ProcessCanRun = true;
                        return true;
                    }
                    else
                    {
                        return HandleRunProcess(date, 5);
                    }

                    return false;


                case "15m":
                    int[] m15 = {00, 15, 30, 45};
                    if (m15.Contains(date.Minute) && date.Second == 00)
                    {
                        ProcessCanRun = true;
                        return true;
                    }
                    else
                    {
                        return HandleRunProcess(date, 15);
                    }

                    return false;

                case "30m":
                    int[] m30 = {00, 30};
                    if (m30.Contains(date.Minute) && date.Second == 00)
                    {
                        ProcessCanRun = true;
                        return true;
                    }
                    else
                    {
                        return HandleRunProcess(date, 30);
                    }

                    return false;

                case "1h":
                    int[] h1 =
                    {
                        00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23
                    };
                    if (h1.Contains(date.Hour) && date.Second == 00)
                    {
                        ProcessCanRun = true;
                        return true;
                    }
                    else
                    {
                        return HandleRunProcess(date, 60);
                    }

                    return false;

                case "4h":
                    int[] h4 = {00, 04, 08, 12, 16, 20};
                    if (h4.Contains(date.Hour) && date.Second == 00)
                    {
                        ProcessCanRun = true;
                        return true;
                    }
                    else
                    {
                        return HandleRunProcess(date, 240);
                    }

                    return false;

                case "1d":

                    if (date.Hour == 00 && date.Minute == 00 && date.Second == 00)
                    {
                        ProcessCanRun = true;
                        return true;
                    }
                    else
                    {
                        return HandleRunProcess(date, 1440);
                    }

                    return false;

                case "1w":
                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        ProcessCanRun = true;
                        return true;
                    }
                    else
                    {
                        return HandleRunProcess(date, 10080);
                    }

                    return false;

                case "1mn":
                    if (date.Day == 0)
                    {
                        ProcessCanRun = true;
                        return true;
                    }
                    else
                    {
                        return HandleRunProcess(date, 43800);
                    }

                    return false;

                default:
                    throw new Exception();
            }
        }

        private bool HandleRunProcess(DateTime date, int i)
        {
            if (InternalNextDate is null) SetInternalNextDate(date, i);
            return CheckProcessCanRun(date);
        }

        private void SetInternalNextDate(DateTime date, int i)
        {
            date = date.AddMinutes(i);
            InternalNextDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute,
                00);
        }

        private bool CheckProcessCanRun(DateTime date)
        {
            if (date >= InternalNextDate)
            {
                ProcessCanRun = true;
                return true;
            }

            return false;
        }

        private int GetCandleIndiceBuilder()
        {
            switch (TimeFrame)
            {
                case "1m":
                    return 1;

                case "5m":
                    return 5;

                case "15m":
                    return 15;

                case "30m":
                    return 30;

                case "1h":
                    return 60;

                case "4h":
                    return 240;

                case "1d":
                    return 1440;

                case "1w":
                    return 10080;

                case "1mn":
                    return 43800;

                default:
                    throw new Exception();
            }
        }
    }
}