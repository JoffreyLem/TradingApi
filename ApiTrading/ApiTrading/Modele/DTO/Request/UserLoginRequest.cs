namespace ApiTrading.Modele.DTO.Request
{
    using System.ComponentModel.DataAnnotations;

    public class UserLoginRequest
    {
        [Required] [EmailAddress] public string Email { get; set; }


        [Required] public string Password { get; set; }
    }
}