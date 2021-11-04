using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class TradesHistoryCommand : BaseCommand
    {
        public TradesHistoryCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getTradesHistory";

        public override string[] RequiredArguments
        {
            get { return new[] {"start", "end"}; }
        }
    }
}