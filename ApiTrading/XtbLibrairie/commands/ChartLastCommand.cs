using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class ChartLastCommand : BaseCommand
    {
        public ChartLastCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getChartLastRequest";

        public override string[] RequiredArguments
        {
            get { return new[] {"info"}; }
        }
    }
}