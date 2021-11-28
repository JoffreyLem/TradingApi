namespace XtbLibrairie.commands
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class CalendarCommand : BaseCommand
    {
        public CalendarCommand(bool prettyPrint)
            : base(new JSONObject(), prettyPrint)
        {
        }

        public override string CommandName => "getCalendar";

        public override string[] RequiredArguments
        {
            get { return new string[] { }; }
        }
    }
}