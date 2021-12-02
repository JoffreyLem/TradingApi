using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class VersionCommand : BaseCommand
    {
        public VersionCommand(JSONObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => "getVersion";

        public override string[] RequiredArguments
        {
            get { return new string[] { }; }
        }
    }
}