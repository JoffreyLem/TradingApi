﻿using Newtonsoft.Json.Linq;

namespace XtbLibrairie.streaming
{
    using JSONObject = JObject;

    internal class TradeRecordsStop
    {
        public override string ToString()
        {
            var result = new JSONObject();
            result.Add("command", "stopTrades");
            return result.ToString();
        }
    }
}