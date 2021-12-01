namespace ApiTrading.Modele.DTO.Response
{
    using System;
    using Microsoft.AspNetCore.Identity;

    public class SubscriptionResponse
    {
        
        public SubscriptionResponse()
        {
            
        }
        public SubscriptionResponse(Subscription subscription)
        {
            Symbol = subscription.Symbol;
            User = subscription.User.UserName;
            LastSignalInfoSend = subscription.LastSignalInfoSend;
        }

        public string Symbol { get; set; }
        public string  User { get; set; }
        public DateTime LastSignalInfoSend { get; set; }
    }
}