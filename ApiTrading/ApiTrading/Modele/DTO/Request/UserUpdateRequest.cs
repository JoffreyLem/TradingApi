using System.ComponentModel.DataAnnotations;

namespace ApiTrading.Modele.DTO.Request
{
    public class UserUpdateRequest 
    {
        [EmailAddress]
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}