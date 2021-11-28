namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TradeRecordsCommand : BaseCommand
    {
        public TradeRecordsCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getTradeRecords";

        public override string[] RequiredArguments
        {
            get { return new[] { "orders" }; }
        }
    }
}