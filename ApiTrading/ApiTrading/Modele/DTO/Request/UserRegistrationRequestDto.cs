using System.ComponentModel.DataAnnotations;

namespace ApiTrading.Modele.DTO.Request
{
    public class UserRegistrationRequestDto
    {
        [Required] public string UserName { get; set; }
        [Required] public string Name { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }

        [Required] public string Password { get; set; }
    }
}