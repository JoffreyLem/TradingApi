namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TradesHistoryCommand : BaseCommand
    {
        public TradesHistoryCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getTradesHistory";

        public override string[] RequiredArguments
        {
            get { return new[] { "start", "end" }; }
        }
    }
}