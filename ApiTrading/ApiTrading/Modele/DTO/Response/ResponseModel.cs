using System.ComponentModel.DataAnnotations;

namespace ApiTrading.Modele.DTO.Response
{
    public class ResponseModel
    {

        public ResponseModel()
        {
            
        }
        protected ResponseModel(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }

        [Required]
        public string? Message { get; set; }
        
        
        [Required]
        public int StatusCode { get; set; }
    }
}