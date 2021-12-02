using System;

namespace ApiTrading.Modele.DTO.Response
{
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
        public string User { get; set; }
        public DateTime LastSignalInfoSend { get; set; }
    }
}