using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Modele;

namespace Utility
{
    public class Log
    {
        private const int rec = 5;
        private readonly string root = @"C:\Users\lemer\OneDrive\Documents\Travail\Projets\Robot trading\Log";


        public Log(string strategyName, string symbol, string timeframe1, string timeframe2, bool isBacktest = false)
        {
            IsBackTest = true;
            StrategyName = strategyName;
            Symbol = symbol;
            Timeframe1 = timeframe1;
            Timeframe2 = timeframe2;
            FileName = $"{StrategyName}_{Symbol}";
            CandleFileName = symbol;
            PositionFileName = $"{StrategyName}_{Symbol}_positions";
            FileNameBacktest = $"{StrategyName}_{Symbol}_{DateTime.Now}_backtest";
            PositionFileNameBacktest = $"{StrategyName}_{Symbol}_positionsBacktest";
            Folder1 = @$"{root}\{StrategyName}\{Symbol}\Real\";
            Folder2 = @$"{root}\{StrategyName}\{Symbol}\Backtest\";
            Init();
        }

        public string StrategyName { get; set; }
        public string Symbol { get; set; }

        public string Timeframe1 { get; set; }

        public string Timeframe2 { get; set; }

        public bool IsBackTest { get; set; }

        private string FileName { get; }

        private string FileNameBacktest { get; }

        private string CandleFileName { get; }

        private string PositionFileName { get; }

        private string PositionFileNameBacktest { get; }

        private string Folder1 { get; }

        private string Folder2 { get; }

        private void Init()
        {
            Directory.CreateDirectory(Folder1);
            Directory.CreateDirectory(Folder2);
        }

        private async Task WriteLogBacktest(string message)
        {
            var filename = $"{Symbol}_{StrategyName}_Backtest_{DateTime.Now:dd-MM-yyyy--hh-mm-ss}.txt";
            var filePath = Folder2 + filename;
            if (!File.Exists(filePath))
                // Create a file to write to.
                using (var sw = File.CreateText(filePath))
                {
                    sw.WriteLine(message);
                }
        }

        private async Task WriteLogStrategy(string message)
        {
        }

        private async Task WriteLogCandle(string message)
        {
        }


        public async void WriteSignal(string IndicatorName, string indicatorLevel, DateTime date, Signal signal)
        {
            var tf = GetTfByIndicatorLevel(indicatorLevel);
            var message = new StringBuilder();
            message.Append(
                $"[{DateTime.Now}] [INDICATOR SIGNAL] Indicateur:{IndicatorName} Market:{Symbol} Timeframe:{tf} Type :{signal.GetEnumDescription()} Date:{date.ToString()}");

            Console.WriteLine(new string('=', rec) + "SIGNAL" + new string('=', rec));
            Console.WriteLine(message);
            Console.WriteLine(new string('=', rec) + new string('=', "SIGNAL".Length) + new string('=', rec));
            Console.WriteLine("");

            await WriteLogStrategy(message.ToString());
        }

        public async void WriteState(string IndicatorName, string indicatorLevel, DateTime date, Signal signal)
        {
            var tf = GetTfByIndicatorLevel(indicatorLevel);
            var message = new StringBuilder();
            message.Append(
                $"[{DateTime.Now}] [STATE] Indicateur:{IndicatorName} Market:{Symbol} Timeframe:{tf} Type :{signal.GetEnumDescription()} Date:{date.ToString()}");
            Console.WriteLine(new string('=', rec) + "STATE" + new string('=', rec));
            Console.WriteLine(message);
            Console.WriteLine(new string('=', rec) + new string('=', "STATE".Length) + new string('=', rec));
            Console.WriteLine("");
            await WriteLogStrategy(message.ToString());
        }

        public async void WriteStrategySignal(DateTime date, Signal signal)
        {
            var message = new StringBuilder();
            message.Append(
                $"[{DateTime.Now}] [STRATEGY SIGNAL] Market:{Symbol} Type :{signal.GetEnumDescription()} Date:{date.ToString()}");
            Console.WriteLine(new string('=', rec) + "SIGNAL MARKET" + new string('=', rec));
            Console.WriteLine(message);
            Console.WriteLine(new string('=', rec) + new string('=', "SIGNAL MARKET".Length) + new string('=', rec));
            Console.WriteLine("");
            await WriteLogStrategy(message.ToString());
        }

        public async void WriteNewCandle(Candle candle, string timeframe)
        {
            var message = new StringBuilder();
            message.Append($"[{DateTime.Now}] [NEW CANDLE] Timeframe:{timeframe} {candle}");

            if (!IsBackTest)
            {
                Console.WriteLine(new string('=', rec) + "NEW CANDLE" + new string('=', rec));
                Console.WriteLine(message);
                Console.WriteLine(new string('=', rec) + new string('=', "NEW CANDLE".Length) + new string('=', rec));
                Console.WriteLine("");
            }


            await WriteLogCandle(message.ToString());
        }


        public async void WritePositionOpened(Position position)
        {
            var message = new StringBuilder();
            message.Append($"[{DateTime.Now}] [POSITION OPENED] {position.ID2} {position}");
            Console.WriteLine(message);
            Console.WriteLine();
            await WriteLogStrategy(message.ToString());
        }

        public async void WritePositionUpdated(Position position)
        {
            var message = new StringBuilder();
            message.Append($"[{DateTime.Now}] [POSITION UPDATED] {position.ID2} {position}");
            Console.WriteLine(message);
            Console.WriteLine();
            await WriteLogStrategy(message.ToString());
        }

        public async void WritePositionClosed(Position position)
        {
            var message = new StringBuilder();
            message.Append($"[{DateTime.Now}] [POSITION CLOSED] {position.ID2} {position}");
            Console.WriteLine(message);
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();
            await WriteLogStrategy(message.ToString());
        }

        public async void WriteResultData(string message)
        {
            Console.WriteLine(message);
            await WriteLogBacktest(message);
        }


        private string GetTfByIndicatorLevel(string level)
        {
            switch (level)
            {
                case "L1":
                    return Timeframe1;
                case "L2":
                    return Timeframe2;
                default:
                    return "";
            }
        }
    }
}