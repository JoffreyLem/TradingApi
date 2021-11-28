namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class TradesCommand : BaseCommand
    {
        public TradesCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getTrades";

        public override string[] RequiredArguments
        {
            get { return new[] { "openedOnly" }; }
        }
    }
}