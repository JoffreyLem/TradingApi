using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class MarginTradeCommand : BaseCommand
    {
        public MarginTradeCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getMarginTrade";

        public override string[] RequiredArguments
        {
            get { return new[] {"symbol", "volume"}; }
        }
    }
}