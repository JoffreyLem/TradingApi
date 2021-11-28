namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class NewsCommand : BaseCommand
    {
        public NewsCommand(JSONObject body, bool prettyPrint)
            : base(body, prettyPrint)
        {
        }

        public override string CommandName => "getNews";

        public override string[] RequiredArguments
        {
            get { return new[] { "start", "end" }; }
        }
    }
}