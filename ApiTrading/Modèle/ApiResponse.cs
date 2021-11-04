namespace Modele
{
    public class ApiResponse
    {
        public ApiResponse(long? dataOrder, bool b)
        {
            Order = dataOrder;
            Status = b;
        }

        public long? Order { get; set; }

        public bool Status { get; set; }
    }
}