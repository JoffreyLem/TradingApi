using System;

namespace Modele
{
    public static class XtbModelHelper
    {
        public static StatusPosition GetStatusPosition(long? type)
        {
            switch (type)
            {
                case 0:
                    return StatusPosition.Open;
                case 1:
                    return StatusPosition.Pending;
                case 2:
                    return StatusPosition.Close;
                default:
                    throw new Exception("");
            }
        }

        public static TypePosition GetTypePosition(long? cmd)
        {
            switch (cmd)
            {
                case 0:
                    return TypePosition.Buy;
                case 1:
                    return TypePosition.Sell;
                case 2:
                    return TypePosition.BuyLimit;
                case 3:
                    return TypePosition.SellLimit;
                case 4:
                    return TypePosition.BuyStop;
                case 5:
                    return TypePosition.SellStop;
                case 6:
                    return TypePosition.Balance;
                case 7:
                    return TypePosition.Credit;
                default:
                    throw new Exception("");
            }
        }

        public static RequestStatus GetRequestStatus(long? id)
        {
            switch (id)
            {
                case 0:
                    return RequestStatus.Error;
                case 1:
                    return RequestStatus.Pending;
                case 2:
                    return RequestStatus.Accepted;
                case 3:
                    return RequestStatus.Rejeted;
                default:
                    throw new Exception("");
            }
        }
    }
}