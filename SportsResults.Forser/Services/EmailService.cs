using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SportsResults.Forser.Models;

namespace SportsResults.Forser.Services
{
    internal class EmailService : IEmailService
    {
        private readonly IOptions<SettingsModel> appSettings;

        public EmailService() { }

        private EmailService(IOptions<SettingsModel> app)
        {
            appSettings = app;
        }

        public void SendEmail(string email)
        {
            MimeMessage newEmail = new MimeMessage();

            newEmail.Subject = "Today's NBA Results";

            using (SmtpClient smtp = new SmtpClient())
            {
                try
                {
                    smtp.Connect(appSettings.Value.EmailServer, 587, MailKit.Security.SecureSocketOptions.StartTls);

                    smtp.Authenticate(appSettings.Value.FromEmail, appSettings.Value.Password);

                    smtp.Send(newEmail);
                    smtp.Disconnect(true);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}