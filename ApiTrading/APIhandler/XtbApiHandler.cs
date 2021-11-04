using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Modele;
using Modele.StramingModel;
using Utility;
using XtbLibrairie.codes;
using XtbLibrairie.commands;
using XtbLibrairie.records;
using XtbLibrairie.sync;

namespace APIhandler
{
    public class XtbApiHandler : ApiHandler
    {
        private static readonly Server serverData = Servers.DEMO;
        private static string userId = "";
        private static string password = "";
        private static string appName = "RobotData <DEMO>";
        private static string appId = "";
        public static SyncAPIConnector connector;

        public XtbApiHandler()
        {
            var credentials = new Credentials(userId, password);
            connector = new SyncAPIConnector(serverData);
            var loginResponse = APICommandFactory.ExecuteLoginCommand(connector, credentials, true);
            ConnectStreaming();
            GetAllSymbol();
            Ping();
        }


        public override string ApiName => "XtbApi";

        public sealed override void SetCredentials()
        {
            if (File.Exists(CredentialFileFolder))
            {
                var lines = File.ReadAllLines(CredentialFileFolder);
                userId = lines[0].Trim();
                password = lines[1].Trim();
            }
            else
            {
                try
                {
                    AskForCredentials();
                }
                catch (Exception e)
                {
                    throw new Exception("Fichier credentials manquant");
                }
            }
        }

        public sealed override void AskForCredentials()
        {
            try
            {
                Console.Write("Login :");
                var login = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Password :");
                var passwd = Console.ReadLine();
                if (!Directory.Exists(CredentialFolder)) Directory.CreateDirectory(CredentialFolder);

                using (var writer = new StreamWriter(CredentialFileFolder))
                {
                    writer.WriteLine(login);
                    writer.WriteLine(passwd);
                }

                SetCredentials();
            }
            catch (Exception e)
            {
            }
        }

        public override async void Ping()
        {
            while (true)
            {
                APICommandFactory.ExecutePingCommand(connector);
                await Task.Delay(TimeSpan.FromMinutes(10)).ConfigureAwait(false);
            }
        }

        public override SymbolInformations GetSymbolInformation(string symbol)
        {
            return AllSymbolList.FirstOrDefault(x => x.Symbol == symbol);
        }

        public override void GetAllSymbol()
        {
            var data = APICommandFactory.ExecuteAllSymbolsCommand(connector);
            AllSymbolList = data.SymbolRecords.Select(x => new SymbolInformations(x)).ToList();
        }

        public override double? CalculerProfit(string symbol, double? volume, TypePosition typePosition,
            double? openPrice, double? closePrice)
        {
            var data = APICommandFactory.ExecuteProfitCalculationCommand(connector, symbol, volume,
                GetTradeOperationByTypePosition(typePosition),
                openPrice, closePrice);
            return data.Profit;
        }

        public override async Task<List<Candle>> GetAllChart(string symbol, string periodCodeStr,
            double? symbolTickSize, bool fullData = false)
        {
            (PERIOD_CODE periodCode, DateTime dateTime) data;
            if (fullData)
                data = SetDateTime(periodCodeStr);
            else
                data = SetDateTimeCourt(periodCodeStr);

            var chartLastResponse = APICommandFactory.ExecuteChartLastCommand(connector, symbol, data.periodCode,
                data.dateTime.ConvertToUnixTime());
            var convertedData = chartLastResponse.RateInfos
                .Where(x => x.Ctm.ConvertToDatetime() > DateTime.Now.AddMonths(-1)).Select(x =>
                    new Candle(x.Open, x.High, x.Low, x.Close, x.Ctm.ConvertToDatetime(), x.Vol, symbolTickSize))
                .ToList();

            return convertedData;
        }

        public override async Task<List<Candle>> GetPartialChart(string symbol, string periodCodeStr,
            double? symbolTickSize, long? start, long? end)
        {
            var data = SetDateTime(periodCodeStr);
            var chartrangeinfo = new ChartRangeInfoRecord(symbol, data.periodCode, start, end, 0);
            var chartLastResponse = APICommandFactory.ExecuteChartRangeCommand(connector, chartrangeinfo);
            return chartLastResponse.RateInfos.Where(x => x.Ctm.ConvertToDatetime() > DateTime.Now.AddMonths(-1))
                .Select(x =>
                    new Candle(x.Open, x.High, x.Low, x.Close, x.Ctm.ConvertToDatetime(), x.Vol, symbolTickSize))
                .ToList();
        }

        public override async Task<ApiResponse> OpenPosition(TypePosition cmd, double? price, double? sl, double? tp,
            string symbol, double? volume,
            long? order, long? expiration, string customComment)
        {
            var ttOpenInfoRecord = new TradeTransInfoRecord(
                GetTradeOperationByTypePosition(cmd),
                TRADE_TRANSACTION_TYPE.ORDER_OPEN,
                price, sl, tp, symbol, volume, order, customComment, expiration);
            try
            {
                var data = APICommandFactory.ExecuteTradeTransactionCommand(connector, ttOpenInfoRecord);
                var apiResult = new ApiResponse(data.Order, true);
                return apiResult;
            }
            catch (Exception e)
            {
                return new ApiResponse(null, true);
            }
        }

        public override async Task<ApiResponse> UpdatePosition(Position tradeRecord,
            PositionInformations positionInformations)
        {
            var type = TRADE_OPERATION_CODE.GetOperationCode((int) tradeRecord.TypePosition);
            var price = positionInformations.Price;
            var sl = positionInformations.Sl;
            var tp = positionInformations.Tp;

            try
            {
                var data = APICommandFactory.ExecuteTradeTransactionCommand(connector,
                    type, TRADE_TRANSACTION_TYPE.ORDER_MODIFY,
                    (double) price, sl, tp, tradeRecord.Symbol, tradeRecord.Volume, tradeRecord.Order,
                    tradeRecord.Comment, 0);
                var apiResult = new ApiResponse(data.Order, true);
                return apiResult;
            }
            catch (Exception e)
            {
                return new ApiResponse(null, true);
            }
        }

        public override async Task<ApiResponse> ClosePosition(Position tradeRecord,
            PositionInformations positionInformations)
        {
            var price = (double?) positionInformations.Price;
            var sl = 0.0;
            var tp = 0.0;
            var symbol = tradeRecord.Symbol;
            var volume = tradeRecord.Volume;
            var order = tradeRecord.Order.GetValueOrDefault();
            string? customComment = null;
            var expiration = 0;
            var typePosition = TRADE_OPERATION_CODE.GetOperationCode((int) tradeRecord.TypePosition);
            var ttCloseInfoRecord = new TradeTransInfoRecord(
                typePosition,
                TRADE_TRANSACTION_TYPE.ORDER_CLOSE,
                price, sl, tp, symbol, volume, order, customComment, expiration);

            try
            {
                var data = APICommandFactory.ExecuteTradeTransactionCommand(connector, ttCloseInfoRecord, true);
                var apiResult = new ApiResponse(data.Order, true);
                return apiResult;
            }
            catch (Exception e)
            {
                return new ApiResponse(null, true);
            }
        }

        public override async Task<AccountInfo> GetAccountInfo()
        {
            var data = APICommandFactory.ExecuteMarginLevelCommand(connector);
            return new AccountInfo(data);
        }

        public override async Task<List<Position>> GetTradeHistory(DateTime start)
        {
            var data = APICommandFactory.ExecuteTradesHistoryCommand(connector, start.ConvertToUnixTime(),
                DateTime.Now.ConvertToUnixTime());
            return data.TradeRecords.Select(x => new Position(x.Symbol, x.Cmd.ConvertCmdToType(), x.Profit,
                x.Close_price, x.Close_time.ConvertToDatetime(), x.Open_price, x.Open_time.ConvertToDatetime(), x.Sl,
                x.Tp, x.Volume, x.CustomComment)).ToList();
        }

        public override async Task<List<Position>> GetTrades()
        {
            var data = APICommandFactory.ExecuteTradesCommand(connector, true);
            return data.TradeRecords.Select(x => new Position(x.Symbol, x.Cmd.ConvertCmdToType(), x.Profit,
                x.Close_price, x.Close_time.ConvertToDatetime(), x.Open_price, x.Open_time.ConvertToDatetime(), x.Sl,
                x.Tp, x.Volume, x.CustomComment)).ToList();
        }

        public override void ConnectStreaming()
        {
            connector.Streaming.Connect();
        }

        public override void DisConnectStreaming()
        {
            connector.Streaming.Disconnect();
        }

        public override void SubscribeCandle(string symbol)
        {
            connector.Streaming.SubscribeCandles(symbol);
        }

        public override void AddCallBackCandleReceived(Action<CandleRecordStreaming> streamingOnTradeRecordReceived)
        {
            connector.Streaming.CandleRecordReceived += streamingOnTradeRecordReceived.Invoke;
        }


        public override void UnsubscribeCandle(string symbol)
        {
            connector.Streaming.UnsubscribeCandles(symbol);
        }

        public override void SubscribeBalance()
        {
            connector.Streaming.SubscribeBalance();
        }

        public override void AddCallBackTradeBalanceReceived(
            Action<BalanceRecordStreaming> streamingOnTradeRecordReceived)
        {
            connector.Streaming.BalanceRecordReceived += streamingOnTradeRecordReceived.Invoke;
        }

        public override void UnsubscribeBalance()
        {
            connector.Streaming.UnsubscribeBalance();
        }

        public override void SubscribeNews()
        {
            connector.Streaming.SubscribeNews();
        }

        public override void AddCallBackNewsReceived(Action<NewsRecordStreaming> streamingOnTradeRecordReceived)
        {
            connector.Streaming.NewsRecordReceived += streamingOnTradeRecordReceived.Invoke;
        }

        public override void UnSubscribeNews()
        {
            connector.Streaming.UnsubscribeNews();
        }

        public override void SubscribeTrade()
        {
            connector.Streaming.SubscribeTrades();
        }

        public override void AddCallBackTradeRecordReceived(Action<TradeRecordStreaming> streamingOnTradeRecordReceived)
        {
            connector.Streaming.TradeRecordReceived += streamingOnTradeRecordReceived.Invoke;
        }


        public override void UnsubscribeTrade()
        {
            connector.Streaming.UnsubscribeTrades();
        }

        public override void SubscribeTradeStatus()
        {
            connector.Streaming.SubscribeTradeStatus();
        }

        public override void AddCallBackTradeStatusRecordReceived(
            Action<TradeStatusStreaming> streamingOnTradeRecordReceived)
        {
            connector.Streaming.TradeStatusRecordReceived += streamingOnTradeRecordReceived.Invoke;
        }

        public override void UnsubscribeTradeStatus()
        {
            connector.Streaming.UnsubscribeTradeStatus();
        }

        public override void SubscribeProfit()
        {
            connector.Streaming.SubscribeProfits();
        }

        public override void AddCallBackProfitReceived(Action<ProfitRecordStreaming> streamingOnTradeRecordReceived)
        {
            connector.Streaming.ProfitRecordReceived += streamingOnTradeRecordReceived.Invoke;
        }

        public override void UnsubscribeProfit()
        {
            connector.Streaming.UnsubscribeProfits();
        }

        public override void SubscribePrice(string symbol)
        {
            connector.Streaming.SubscribePrice(symbol);
        }

        public override void AddCallBackPriceReceived(Action<TickRecordStreaming> streamingOnTradeRecordReceived)
        {
            connector.Streaming.TickRecordReceived += streamingOnTradeRecordReceived.Invoke;
        }

        public override void UnsubscribePrice(string symbol)
        {
            connector.Streaming.UnsubscribePrice(symbol);
        }

        private static (PERIOD_CODE periodCode, DateTime dateTime) SetDateTime(string periodCode)
        {
            DateTime dateTime;
            switch (periodCode)
            {
                case "1m":
                    dateTime = DateTime.Now.AddMonths(-1);
                    return (PERIOD_CODE.PERIOD_M1, dateTime);

                case "5m":
                    dateTime = DateTime.Now.AddMonths(-1);
                    return (PERIOD_CODE.PERIOD_M5, dateTime);

                case "15m":
                    dateTime = DateTime.Now.AddMonths(-7);
                    return (PERIOD_CODE.PERIOD_M15, dateTime);

                case "30m":
                    dateTime = DateTime.Now.AddMonths(-7);
                    return (PERIOD_CODE.PERIOD_M30, dateTime);

                case "1h":
                    dateTime = DateTime.Now.AddMonths(-7);
                    return (PERIOD_CODE.PERIOD_H1, dateTime);

                case "4h":
                    dateTime = DateTime.Now.AddMonths(-7);
                    return (PERIOD_CODE.PERIOD_H4, dateTime);

                case "1d":
                    dateTime = DateTime.Now.AddMonths(-13);
                    return (PERIOD_CODE.PERIOD_D1, dateTime);

                case "1w":
                    dateTime = DateTime.Now.AddMonths(-13);
                    return (PERIOD_CODE.PERIOD_W1, dateTime);

                case "1mn":
                    dateTime = DateTime.Now.AddMonths(-13);
                    return (PERIOD_CODE.PERIOD_MN1, dateTime);

                default:
                    throw new Exception("Periode code n'existe pas");
            }
        }

        private static (PERIOD_CODE periodCode, DateTime dateTime) SetDateTimeCourt(string periodCode)
        {
            DateTime dateTime;
            switch (periodCode)
            {
                case "1m":
                    dateTime = DateTime.Now.AddMinutes(-200);
                    return (PERIOD_CODE.PERIOD_M1, dateTime);

                case "5m":
                    dateTime = DateTime.Now.AddMinutes(-1000);
                    return (PERIOD_CODE.PERIOD_M5, dateTime);

                case "15m":
                    dateTime = DateTime.Now.AddMinutes(-3120);
                    return (PERIOD_CODE.PERIOD_M15, dateTime);

                case "30m":
                    dateTime = DateTime.Now.AddMinutes(-6000);
                    return (PERIOD_CODE.PERIOD_M30, dateTime);

                case "1h":
                    dateTime = DateTime.Now.AddHours(-200);
                    return (PERIOD_CODE.PERIOD_H1, dateTime);

                case "4h":
                    dateTime = DateTime.Now.AddHours(-800);
                    return (PERIOD_CODE.PERIOD_H4, dateTime);

                case "1d":
                    dateTime = DateTime.Now.AddDays(-200);
                    return (PERIOD_CODE.PERIOD_D1, dateTime);

                case "1w":
                    dateTime = DateTime.Now.AddDays(-1400);
                    return (PERIOD_CODE.PERIOD_W1, dateTime);

                case "1mn":
                    dateTime = DateTime.Now.AddMonths(-13);
                    return (PERIOD_CODE.PERIOD_MN1, dateTime);

                default:
                    throw new Exception("Periode code n'existe pas");
            }
        }

        public TRADE_OPERATION_CODE? GetTradeOperationBySignal(Signal signal)
        {
            if (signal == Signal.Buy)
                return TRADE_OPERATION_CODE.BUY;
            if (signal == Signal.Sell)
                return TRADE_OPERATION_CODE.SELL;
            return null;
        }

        public TRADE_OPERATION_CODE? GetTradeOperationByTypePosition(TypePosition signal)
        {
            if (signal == TypePosition.Buy)
                return TRADE_OPERATION_CODE.BUY;
            if (signal == TypePosition.Sell)
                return TRADE_OPERATION_CODE.SELL;
            return null;
        }
    }
}