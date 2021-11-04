using System.ComponentModel.DataAnnotations;

namespace ApiTrading.Modele.DTO.Request
{
    public class ApiConnectionRequestDto
    {
        [Required]
        public string Account { get; set; }
        

        [Required]
        public string Password { get; set; }
    }
}