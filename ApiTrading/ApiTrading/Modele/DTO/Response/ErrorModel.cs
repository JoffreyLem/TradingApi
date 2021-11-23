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
    public class ErrorModel 
    {
        [JsonProperty(Order = 3)]
        public List<string> ErrorMessage { get; set; }

  
        public ErrorModel(int statusCode,List<string> errorMessage)
        {
    
            ErrorMessage = errorMessage;
        }
        
        public ErrorModel()
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