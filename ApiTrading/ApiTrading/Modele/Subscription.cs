namespace ApiTrading.Modele
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Identity;

    public class Subscription
    {
        public string Symbol { get; set; }
      
        public IdentityUser<int> User { get; set; }
        public DateTime LastSignalInfoSend { get; set; }
    }
}