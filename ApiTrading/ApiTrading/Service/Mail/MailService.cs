using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ApiTrading.Service.Mail
{
    public class MailService : IMail
    {
        private readonly bool _enableSsl;
        private readonly string _fromAddress;
        private readonly string _fromAddressTitle;
        private readonly string _password;
        private readonly int _smtpPort;
        private readonly string _smtpServer;
        private readonly string _username;
        private bool _useDefaultCredentials;

        public
            MailService(IConfiguration configuration) // configuration is automatically added to DI in ASP.NET Core 3.0
        {
            _smtpServer = configuration["Email:SmtpServer"];
            _smtpPort = int.Parse(configuration["Email:SmtpPort"]);
            _smtpPort = _smtpPort == 0 ? 25 : _smtpPort;
            _fromAddress = configuration["Email:FromAddress"];
            _fromAddressTitle = configuration["FromAddressTitle"];
            _username = configuration["Email:SmtpUsername"];
            _password = configuration["Email:SmtpPassword"];
            _enableSsl = bool.Parse(configuration["Email:EnableSsl"]);
            _useDefaultCredentials = bool.Parse(configuration["Email:UseDefaultCredentials"]);
        }

        public async Task Send(string toAddress, string subject, string body, bool sendAsync = true)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_fromAddressTitle, _fromAddress));
            mimeMessage.To.Add(new MailboxAddress(toAddress));
            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect(_smtpServer, _smtpPort, _enableSsl);
                client.Authenticate(_username,
                    _password); // If using GMail this requires turning on LessSecureApps : https://myaccount.google.com/lesssecureapps
                if (sendAsync)
                    await client.SendAsync(mimeMessage);
                else
                    client.Send(mimeMessage);

                client.Disconnect(true);
            }
        }
    }
}