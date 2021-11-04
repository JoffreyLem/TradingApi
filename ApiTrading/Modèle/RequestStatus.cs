using System.ComponentModel;

namespace Modele
{
    public enum RequestStatus
    {
        [Description("Error")] Error,
        [Description("Pending")] Pending,
        [Description("Accepted")] Accepted,
        [Description("Rejeted")] Rejeted
    }
}