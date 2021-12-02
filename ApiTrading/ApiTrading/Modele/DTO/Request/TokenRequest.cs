using System.ComponentModel.DataAnnotations;

namespace ApiTrading.Modele.DTO.Request
{
    public class TokenRequest
    {
        [Required] public string Token { get; set; }

        [Required] public string RefreshToken { get; set; }
    }
}