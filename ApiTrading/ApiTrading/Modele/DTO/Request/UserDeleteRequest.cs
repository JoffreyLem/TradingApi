using System.ComponentModel.DataAnnotations;

namespace ApiTrading.Modele.DTO.Request
{
    public class UserDeleteRequest
    {
        [Required]
        public string Email { get; set; }
    }
}