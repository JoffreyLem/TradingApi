using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

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