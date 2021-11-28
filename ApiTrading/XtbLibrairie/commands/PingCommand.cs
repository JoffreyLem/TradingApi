namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class PingCommand : BaseCommand
    {
        public PingCommand(bool? prettyPrint)
            : base(new JSONObject(), prettyPrint)
        {
        }

        public override string CommandName => "ping";

        public override string[] RequiredArguments
        {
            get { return new string[] { }; }
        }
    }
}