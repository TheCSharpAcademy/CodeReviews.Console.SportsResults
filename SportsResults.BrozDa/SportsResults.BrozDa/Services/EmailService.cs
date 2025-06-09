using SportsResults.BrozDa.Models;
using System.Net.Mail;

namespace SportsResults.BrozDa.Services
{
    /// <summary>
    /// Handles sending emails with game results using SMTP.
    /// </summary>
    internal class EmailService
    {
        SmtpSettings SmtpSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="smtpSettings">The SMTP settings containing Host, port, sender and recipient</param>
        public EmailService(SmtpSettings smtpSettings)
        {
            SmtpSettings = smtpSettings;
        }

        /// <summary>
        /// Sends an email containing the provided body text with the current date as the subject.
        /// </summary>
        /// <param name="body">The content of the email message.</param>
        public void Send(string body)
        {
            using MailMessage message = new MailMessage();
            message.From = new MailAddress(SmtpSettings.Sender);
            message.To.Add(SmtpSettings.Recipient);
            message.Subject = $"NBA games result for {DateTime.Now.ToString("d MMM yyyy")}";
            message.Body = body;

            using SmtpClient smtp = new SmtpClient(SmtpSettings.Host, SmtpSettings.Port);

            smtp.Send(message);

            Console.WriteLine("Email sent!");
        }
    }
}