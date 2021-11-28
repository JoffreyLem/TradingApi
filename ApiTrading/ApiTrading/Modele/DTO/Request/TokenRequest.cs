namespace ApiTrading.Modele.DTO.Request
{
    using System.ComponentModel.DataAnnotations;

    public class TokenRequest
    {
        [Required] public string Token { get; set; }

        [Required] public string RefreshToken { get; set; }
    }
}