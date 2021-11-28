namespace ApiTrading.Modele.DTO.Request
{
    using System.ComponentModel.DataAnnotations;

    public class UserLoginRequest
    {
        [Required] public string Login { get; set; }


        [Required] public string Password { get; set; }
    }
}