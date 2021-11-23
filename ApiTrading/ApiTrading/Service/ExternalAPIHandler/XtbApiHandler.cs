using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiTrading.Exception;
using ApiTrading.Modele.DTO.Response;
using ApiTrading.Service.ExternalAPIHandler;
using Modele;
using Modele.StramingModel;
using Utility;
using XtbLibrairie.codes;
using XtbLibrairie.commands;
using XtbLibrairie.records;
using XtbLibrairie.responses;
using XtbLibrairie.sync;
using BaseResponse = ApiTrading.Modele.DTO.Response.BaseResponse;

namespace APIhandler
{
    public class XtbApiHandler : IApiHandler
    {
        private  readonly Server serverData = Servers.DEMO;
        private  string userId = "";
        private  string password = "";
        private  string appName = "RobotData <DEMO>";
        private  string appId = "";
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
            return new BaseResponse("Logout API XTB Ok");
        }

        public SyncAPIConnector connector { get; set; }
        public  string ApiName { get => "XtbApi"; }
        public virtual string CredentialFileName => ApiName + "Credential.txt";
        public string CredentialFileFolder =>
            AppDomain.CurrentDomain.BaseDirectory + @"CredentialsFolder/" + CredentialFileName;
        public string CredentialFolder =>
            AppDomain.CurrentDomain.BaseDirectory + @"CredentialsFolder";
        public XtbApiHandler()
        {
            
        }


    

        
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

        public  void GetAllSymbol()
        {
            var data = APICommandFactory.ExecuteAllSymbolsCommand(connector);
            AllSymbolList = data.SymbolRecords.Select(x => new SymbolInformations(x)).ToList();
        }

  

        public  async Task<BaseResponse<List<Candle>>> GetAllChart(string symbol, string periodCodeStr,
            double? symbolTickSize, bool fullData = true)
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
                .Where(x => x.Ctm.ConvertToDatetime() > DateTime.Now.AddMonths(-1)).Select(x =>
                    new Candle(x.Open, x.High, x.Low, x.Close, x.Ctm.ConvertToDatetime(), x.Vol,ratioConverted))
                .ToList();

            var response = new BaseResponse<List<Candle>>(convertedData);

            return response;
        }

        public  async Task<BaseResponse<List<Candle>>> GetPartialChart(string symbol, string periodCodeStr,
            double? symbolTickSize, long? start, long? end)
        {
            var data = SetDateTime(periodCodeStr);
            var chartrangeinfo = new ChartRangeInfoRecord(symbol, data.periodCode, start, end, 0);
            var chartLastResponse = APICommandFactory.ExecuteChartRangeCommand(connector, chartrangeinfo);
            var data2= chartLastResponse.RateInfos.Where(x => x.Ctm.ConvertToDatetime() > DateTime.Now.AddMonths(-1))
                .Select(x =>
                    new Candle(x.Open, x.High, x.Low, x.Close, x.Ctm.ConvertToDatetime(), x.Vol, symbolTickSize))
                .ToList();
            var response = new BaseResponse<List<Candle>>(data2);

            return response;
        }

       

        public  async Task<BaseResponse<AccountInfo>> GetAccountInfo()
        {
            var data = APICommandFactory.ExecuteMarginLevelCommand(connector);
            var account = new AccountInfo(data);
            return new BaseResponse<AccountInfo>(account);
        }


        public List<SymbolInformations> AllSymbolList { get; set; }


      

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