namespace Modele
{
    public sealed class SymbolInformations
    {
        public SymbolInformations()
        {
        }

        public SymbolInformations(dynamic symbolRecord)
        {
            Spread = symbolRecord.SpreadRaw ?? symbolRecord.SpreadTable;
            CategoryName = symbolRecord.CategoryName;
            ContractSize = symbolRecord.ContractSize;
            Currency = symbolRecord.Currency;
            CurrencyPair = symbolRecord.CurrencyPair;
            Precision = symbolRecord.Precision;
            CurrencyProfit = symbolRecord.CurrencyProfit;
            Description = symbolRecord.Description;
            GroupName = symbolRecord.GroupName;
            High = symbolRecord.High;
            InitialMargin = symbolRecord.InitialMargin;
            InstantMaxVolume = symbolRecord.InstantMaxVolume;
            Leverage = symbolRecord.Leverage;
            LotMax = symbolRecord.LotMax;
            LotMin = symbolRecord.LotMin;
            LotStep = symbolRecord.LotStep;
            Low = symbolRecord.Low;
            MarginHedged = symbolRecord.MarginHedged;
            MarginHedgedStrong = symbolRecord.MarginHedgedStrong;
            StopsLevel = symbolRecord.StopsLevel;
            Symbol = symbolRecord.Symbol;
            TickSize = symbolRecord.TickSize;
            TickValue = symbolRecord.TickValue;
            Type = symbolRecord.Type;
        }


        public double? Spread { get; set; }

        public double? Ask { get; set; }

        public double? Bid { get; set; }

        public string CategoryName { get; set; }

        public long? ContractSize { get; set; }

        public string Currency { get; set; }

        public bool? CurrencyPair { get; set; }

        public string CurrencyProfit { get; set; }

        public string Description { get; set; }

        public string GroupName { get; set; }

        public double? High { get; set; }

        public long? InitialMargin { get; set; }

        public long? InstantMaxVolume { get; set; }

        public double? Leverage { get; set; }


        public double? LotMax { get; set; }

        public double? LotMin { get; set; }

        public double? LotStep { get; set; }

        public double? Low { get; set; }

        public long? MarginHedged { get; set; }

        public bool? MarginHedgedStrong { get; set; }

        public long? Precision { get; set; }
        public long? StopsLevel { get; set; }

        public string Symbol { get; set; }

        public double? TickSize { get; set; }

        public double? TickValue { get; set; }

        public long? Time { get; set; }

        public string TimeString { get; set; }

        public long? Type { get; set; }
    }
}