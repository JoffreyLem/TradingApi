namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class SymbolCommand : BaseCommand
    {
        public SymbolCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getSymbol";

        public override string[] RequiredArguments
        {
            get { return new[] { "symbol" }; }
        }
    }
}