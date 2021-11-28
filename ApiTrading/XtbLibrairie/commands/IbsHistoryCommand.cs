namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class IbsHistoryCommand : BaseCommand
    {
        public IbsHistoryCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getIbsHistory";

        public override string[] RequiredArguments
        {
            get { return new[] { "start", "end" }; }
        }
    }
}