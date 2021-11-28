namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class MarginLevelCommand : BaseCommand
    {
        public MarginLevelCommand(bool? prettyPrint) : base(new JSONObject(), prettyPrint)
        {
        }

        public override string CommandName => "getMarginLevel";

        public override string[] RequiredArguments
        {
            get { return new string[] { }; }
        }
    }
}