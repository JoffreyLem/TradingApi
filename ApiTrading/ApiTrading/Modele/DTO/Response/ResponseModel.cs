using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ApiTrading.Modele.DTO.Response
{
    public class ResponseModel
    {

        public ResponseModel()
        {
            
        }
        public ResponseModel(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }
        [JsonProperty(Order = 2)]
        [Required]
        public string? Message { get; set; }
        
        [JsonProperty(Order = 1)]
        [Required]
        public int StatusCode { get; set; }
    }
}