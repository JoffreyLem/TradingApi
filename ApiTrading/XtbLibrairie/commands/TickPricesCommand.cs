using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class TickPricesCommand : BaseCommand
    {
        public TickPricesCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getTickPrices";

        public override string[] RequiredArguments
        {
            get { return new[] {"symbols", "timestamp"}; }
        }
    }
}