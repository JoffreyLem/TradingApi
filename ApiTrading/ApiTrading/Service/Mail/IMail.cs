using System.Threading.Tasks;

namespace ApiTrading.Service.Mail
{
    public interface IMail
    {
        public Task Send(string toAddress, string subject, string body, bool sendAsync = true);
    }
}