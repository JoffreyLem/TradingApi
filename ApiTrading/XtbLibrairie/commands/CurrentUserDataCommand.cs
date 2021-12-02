using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class CurrentUserDataCommand : BaseCommand
    {
        public CurrentUserDataCommand(bool prettyPrint) : base(new JSONObject(), prettyPrint)
        {
        }

        public override string CommandName => "getCurrentUserData";

        public override string[] RequiredArguments
        {
            get { return new string[] { }; }
        }
    }
}