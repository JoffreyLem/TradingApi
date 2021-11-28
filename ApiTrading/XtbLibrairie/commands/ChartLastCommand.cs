namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ChartLastCommand : BaseCommand
    {
        public ChartLastCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getChartLastRequest";

        public override string[] RequiredArguments
        {
            get { return new[] { "info" }; }
        }
    }
}