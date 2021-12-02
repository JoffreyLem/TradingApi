using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class TradeTransactionCommand : BaseCommand
    {
        public TradeTransactionCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "tradeTransaction";

        public override string[] RequiredArguments
        {
            get { return new[] {"tradeTransInfo"}; }
        }
    }
}