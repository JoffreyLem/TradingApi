using Newtonsoft.Json.Linq;

namespace XtbLibrairie.commands
{
    using JSONObject = JObject;

    public class StepRulesCommand : BaseCommand
    {
        public StepRulesCommand()
            : base(new JSONObject(), false)
        {
        }

        public override string CommandName => "getStepRules";

        public override string[] RequiredArguments
        {
            get { return new string[] { }; }
        }
    }
}