using System.ComponentModel.DataAnnotations;

namespace ApiTrading.Modele.DTO.Request
{
    public class XtbLoginRequest
    {
        [Required] public string Login { get; set; }


        [Required] public string Password { get; set; }
    }
}