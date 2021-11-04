namespace Modele
{
    public class AccountInfo
    {
        public AccountInfo(double? balance, double? equity, double? margin, double? marginFree, double? marginLevel,
            string currency, double? credit)
        {
            Balance = balance;
            Equity = equity;
            Margin = margin;
            MarginFree = marginFree;
            MarginLevel = marginLevel;
            Currency = currency;
            Credit = credit;
        }

        public AccountInfo(dynamic marginLevelResponse)
        {
            Balance = marginLevelResponse.Balance;
            Credit = marginLevelResponse.Credit;
            Equity = marginLevelResponse.Equity;
            Margin = marginLevelResponse.Margin;
            MarginFree = marginLevelResponse.Margin_free;
            MarginLevel = marginLevelResponse.Margin_level;
            Currency = marginLevelResponse.Currency;
            Credit = marginLevelResponse.Credit;
        }

        public double? Balance { get; }
        public double? Equity { get; }
        public double? Margin { get; }
        public double? MarginFree { get; }
        public double? MarginLevel { get; }
        public string Currency { get; }
        public double? Credit { get; }
    }
}