namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TradeTransactionStatusCommand : BaseCommand
    {
        public TradeTransactionStatusCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "tradeTransactionStatus";

        public override string[] RequiredArguments
        {
            get { return new[] { "order" }; }
        }
    }
}