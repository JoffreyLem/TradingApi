using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiTrading.Modele.DTO.Request
{
    using System.ComponentModel;

    public class UserLoginRequest
    {
   
        [Required]
        [DefaultValue("test")]
        public string Login { get; set; }
        

        [Required]
        public string Password { get; set; }

     
    }
}