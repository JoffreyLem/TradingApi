namespace ApiTrading.Modele.DTO.Request
{
    using System.ComponentModel.DataAnnotations;

    public class UserUpdateRequest
    {
        [EmailAddress] public string Email { get; set; }
        
        public string UserName { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}