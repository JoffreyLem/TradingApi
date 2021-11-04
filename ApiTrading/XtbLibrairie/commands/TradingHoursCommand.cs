using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class TradingHoursCommand : BaseCommand
    {
        public TradingHoursCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getTradingHours";

        public override string[] RequiredArguments
        {
            get { return new[] {"symbols"}; }
        }
    }
}