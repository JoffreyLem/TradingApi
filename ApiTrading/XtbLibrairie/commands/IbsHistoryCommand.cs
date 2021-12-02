using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class IbsHistoryCommand : BaseCommand
    {
        public IbsHistoryCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getIbsHistory";

        public override string[] RequiredArguments
        {
            get { return new[] {"start", "end"}; }
        }
    }
}