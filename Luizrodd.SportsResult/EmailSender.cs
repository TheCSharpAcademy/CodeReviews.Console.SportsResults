using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Luizrodd.SportsResult
{
    public class EmailSender
    {
        private readonly string SmtpHost;
        private readonly string SmtpPort;
        private readonly string SmtpFrom;
        private readonly string SmtpTo;
        public EmailSender(IConfiguration configuration)
        {
            SmtpHost = configuration["EmailSettings:Host"];
            SmtpPort = configuration["EmailSettings:Port"];
            SmtpFrom = configuration["EmailSettings:From"];
            SmtpTo = configuration["EmailSettings:To"];
        }
        public void SendEmail(string body, string subject = "Basketball Games")
        {
            var smtp = new SmtpClient(SmtpHost, int.Parse(SmtpPort));

            smtp.Send(SmtpFrom, SmtpTo, subject, body);
        }
    }
}
