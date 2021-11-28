namespace ApiTrading.Service.Mail
{
    using System.Threading.Tasks;

    public interface IMail
    {
        public Task Send(string toAddress, string subject, string body, bool sendAsync = true);
    }
}