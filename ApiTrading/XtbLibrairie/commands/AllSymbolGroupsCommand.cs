namespace XtbLibrairie.commands
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class AllSymbolGroupsCommand : BaseCommand
    {
        [Obsolete("Not available in API any more")]
        public AllSymbolGroupsCommand(bool? prettyPrint)
            : base(new JSONObject(), prettyPrint)
        {
        }

        public override string CommandName => "getAllSymbolGroups";

        public override string[] RequiredArguments
        {
            get { return new string[] { }; }
        }
    }
}