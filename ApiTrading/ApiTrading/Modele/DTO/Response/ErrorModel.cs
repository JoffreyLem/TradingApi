using System.Collections.Generic;
using ApiTrading.Helper;
using Newtonsoft.Json;

namespace ApiTrading
{
    public class ErrorModel
    {
        public ErrorModel(int statusCode, List<string> errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ErrorModel()
        {
            ErrorMessage = new List<string>();
        }

        [JsonProperty(Order = 3)] public List<string> ErrorMessage { get; set; }


        public override string ToString()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new OrderedContractResolver()
            };

            return JsonConvert.SerializeObject(this, Formatting.Indented, settings);
        }
    }
}