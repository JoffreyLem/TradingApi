using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class ChartRangeCommand : BaseCommand
    {
        public ChartRangeCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getChartRangeRequest";

        public override string[] RequiredArguments
        {
            get { return new[] {"info"}; }
        }
    }
}