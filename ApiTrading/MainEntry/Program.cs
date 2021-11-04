using System;
using System.Threading.Tasks;
using APIhandler;
using Modele;

namespace MainEntry
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.Title = "test";
            ApiHandler h = new XtbApiHandler();
            var forceStr =
                new ForceStrategy.ForceStrategy("EURUSD", Timeframe.OneMinute, Timeframe.FiveMinutes, h, false);
            //var scalpingStrategy = new ScalpingStrategy("DE30", Timeframe.FifteenMinutes, Timeframe.OneHour, false);
            //IBacktest backtest2 = new MetatraderHandler();
            //BacktestModule backtest = new BacktestModule(typeof(ForceStrategy), "EURUSD", Timeframe.FifteenMinutes,
            //    Timeframe.OneHour, backtest2);

            //backtest.RunBacktestFullData();

            //var data =await XtbHandler.GetTradeHistory2(DateTime.Now.AddYears(-5));
            //var result = new Result(data);

            //result.PrintResult(1000);
            //result.PrintDetailledResult(1000);
        }
    }
}