namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ProfitCalculationCommand : BaseCommand
    {
        public ProfitCalculationCommand(JSONObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getProfitCalculation";

        public override string[] RequiredArguments
        {
            get { return new[] { "cmd", "symbol", "volume", "openPrice", "closePrice" }; }
        }
    }
}