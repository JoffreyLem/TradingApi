using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ApiTrading.Helper;
using ApiTrading.Modele.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ApiTrading
{
    public class ErrorModel :ResponseModel
    {
        [JsonProperty(Order = 3)]
        public List<string> ErrorMessage { get; set; }

        public ErrorModel()
        {
            ErrorMessage = new List<string>();
        }
        public ErrorModel(int statusCode,List<string> errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
        
        public ErrorModel(int statusCode, string message) : base(statusCode,message)
        {
            ErrorMessage = new List<string>();

        }

     

        public override string ToString()
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new OrderedContractResolver()
            };
   
            return JsonConvert.SerializeObject(this, Formatting.Indented,settings);
        }

       
    }
}