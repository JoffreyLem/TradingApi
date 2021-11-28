namespace ApiTrading.Modele
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Identity;

    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        
        public string Symbol { get; set; }
      
        public IdentityUser<int> User { get; set; }
        public DateTime LastSignalInfoSend { get; set; }
    }
}