using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTrading.Exception;
using ApiTrading.Modele.DTO.Response;
using ApiTrading.Service.ExternalAPIHandler;
using Modele;
using Utility;
using XtbLibrairie.codes;
using XtbLibrairie.commands;
using XtbLibrairie.records;
using XtbLibrairie.sync;

namespace APIhandler
{
    public class XtbApiHandler : IApiHandler
    {
        private readonly Server serverData = Servers.DEMO;
        private string appId = "";
        private string appName = "RobotData <DEMO>";
        private string password = "";
        private string userId = "";

        public XtbApiHandler()
        {
            GetAllSymbol();
        }

        public async Task<BaseResponse> Login(string user, string passwordData)
        {
            userId = user;
            password = passwordData;
            var credentials = new Credentials(userId, password);
            connector = new SyncAPIConnector(serverData);
            var loginResponse = APICommandFactory.ExecuteLoginCommand(connector, credentials, true);

            //ConnectStreaming();
            GetAllSymbol();
            Ping();
            return new BaseResponse("Connection à l'api XTB Ok");
        }

        public async Task<BaseResponse> Logout()
        {
            var logOutResponse = APICommandFactory.CreateLogoutCommand();
            connector = null;
            return new BaseResponse("Logout API XTB Ok");
        }

        public SyncAPIConnector connector { get; set; }

        public async Task<BaseResponse<List<SymbolResponse>>> GetAllSymbol()
        {
            var data = APICommandFactory.ExecuteAllSymbolsCommand(connector);
            var rsp = new BaseResponse<List<SymbolResponse>>();
            AllSymbolList = data.SymbolRecords.Select(x => new SymbolInformations(x)).ToList();
            rsp.Data = data.SymbolRecords.Select(x => new SymbolResponse(x.Symbol, x.Description)).ToList();

            return rsp;
        }

        public async Task<BaseResponse<bool>> CheckIfSymbolExist(string symbol)
        {
            var data = APICommandFactory.ExecuteSymbolCommand(connector, symbol);
            var rsp = new BaseResponse<bool>();
            if (data is null)
            {
                rsp.Data = false;
                rsp.Message = "Le symbole n'existe pas";
            }
            else
            {
                rsp.Data = true;
                rsp.Message = "Le symbole existe";
            }

            return rsp;
        }

        public async Task<BaseResponse<List<Candle>>> GetChart(string symbol, string periodCodeStr,
            string? start, string? end)
        {
            if (start is null && end is null)
                return await GetAllChart(symbol, periodCodeStr);
            return await GetPartialChart(symbol, periodCodeStr, start, end);
        }


        public async Task<BaseResponse<List<Candle>>> GetAllChart(string symbol, string periodCodeStr,
            bool fullData = true)
        {
            (PERIOD_CODE periodCode, DateTime dateTime) data;
            if (fullData)
                data = SetDateTime(periodCodeStr);
            else
                data = SetDateTimeCourt(periodCodeStr);

            var chartLastResponse = APICommandFactory.ExecuteChartLastCommand(connector, symbol, data.periodCode,
                data.dateTime.ConvertToUnixTime());
            var ratioConverted = GetSymbolInformation(symbol).TickSize;
            var convertedData = chartLastResponse.RateInfos
                .Select(x =>
                    new Candle(x.Open, x.High, x.Low, x.Close, x.Ctm.ConvertToDatetime(), x.Vol, ratioConverted))
                .ToList();
            if (convertedData.Count == 0)
                return new BaseResponse<List<Candle>>("Pas de données disponible", convertedData);
            var response = new BaseResponse<List<Candle>>(convertedData);

            return response;
        }

        public async Task<BaseResponse<List<Candle>>> GetPartialChart(string symbol, string periodCodeStr,
            string? start, string? end)
        {
            var dateTuple = DateControl(start, end);
            var endtest = dateTuple.end;
            var data = SetDateTime(periodCodeStr);
            var ratioConverted = GetSymbolInformation(symbol).TickSize;
            var chartrangeinfo = new ChartRangeInfoRecord(symbol, data.periodCode, dateTuple.start.ConvertToUnixTime(),
                dateTuple.end?.ConvertToUnixTime(), 0);
            var chartLastResponse = APICommandFactory.ExecuteChartRangeCommand(connector, chartrangeinfo);
            var data2 = chartLastResponse.RateInfos
                .Select(x =>
                    new Candle(x.Open, x.High, x.Low, x.Close, x.Ctm.ConvertToDatetime(), x.Vol, ratioConverted))
                .ToList();

            if (data2.Count == 0) return new BaseResponse<List<Candle>>("Pas de données pour cette période", data2);
            return new BaseResponse<List<Candle>>(data2);
        }


        public List<SymbolInformations> AllSymbolList { get; set; }

        public async void Ping()
        {
            while (true)
            {
                APICommandFactory.ExecutePingCommand(connector);
                await Task.Delay(TimeSpan.FromMinutes(10)).ConfigureAwait(false);
            }
        }

        public SymbolInformations? GetSymbolInformation(string symbol)
        {
            return AllSymbolList.FirstOrDefault(x => x.Symbol == symbol);
        }

        private (DateTime start, DateTime? end) DateControl(string start, string? end)
        {
            DateTime dateStart = default;
            DateTime dateEnd = default;


            start = start ?? throw new FormatDateException("L'argument start ne doit pas être vide");
            if (!DateTime.TryParse(start, out dateStart))
                throw new FormatDateException("La start date n'est pas au bon format");
            if (end is not null)
            {
                if (!DateTime.TryParse(end, out dateEnd))
                    throw new FormatDateException("La start date n'est pas au bon format");
                if (dateEnd < dateStart)
                    throw new InvalideDateRangeException("La end date doit être supérieur à la start date");
                return (dateStart, dateEnd);
            }

            return (dateStart, null);
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
                    var message = "le timeframe n'existe pas";
                    throw new TimeFrameDontExistException(message);
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
    }
}