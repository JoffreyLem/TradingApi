using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ApiTrading.Modele.DTO.Response;
using Microsoft.AspNetCore.Mvc;

namespace ApiTrading
{
    public class ErrorModel :ResponseModel
    {
        public List<string> ErrorMessage { get; set; }

        public ErrorModel()
        {
            
        }
        public ErrorModel(int statusCode,List<string> errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
        
        public ErrorModel(int statusCode, string message) : base(statusCode,message)
        {
        
        }

     

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

       
    }
}