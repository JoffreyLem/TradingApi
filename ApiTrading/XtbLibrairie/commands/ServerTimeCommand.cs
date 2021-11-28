namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class ServerTimeCommand : BaseCommand
    {
        public ServerTimeCommand(bool? prettyPrint) : base(new JSONObject(), prettyPrint)
        {
        }

        public override string CommandName => "getServerTime";

        public override string[] RequiredArguments
        {
            get { return new string[] { }; }
        }
    }
}