using System.Net.Mail;

namespace SportsResults.BrozDa.Services
{
    /// <summary>
    /// Handles sending emails with game results using SMTP.
    /// </summary>
    internal class EmailService
    {
        public string SmtpClientHost { get; set; }
        public int SmtpClientPort { get; set; }
        public string SourceEmail { get; set; }
        public string DestinationEmail {  get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="smtpClientHost">The SMTP server host.</param>
        /// <param name="smtpClientPort">The SMTP server port.</param>
        /// <param name="sourceEmail">The sender's email address.</param>
        /// <param name="destinationEmail">The recipient's email address.</param>
        public EmailService(string smtpClientHost, int smtpClientPort, string sourceEmail, string destinationEmail)
        {
            SmtpClientHost = smtpClientHost;
            SmtpClientPort = smtpClientPort;
            SourceEmail = sourceEmail;
            DestinationEmail = destinationEmail;
        }
        /// <summary>
        /// Sends an email containing the provided body text with the current date as the subject.
        /// </summary>
        /// <param name="body">The content of the email message.</param>
        public void Send(string body)
        {

            using MailMessage message = new MailMessage();
            message.From = new MailAddress(SourceEmail);
            message.To.Add(DestinationEmail);
            message.Subject = $"NBA games result for {DateTime.Now.ToString("d MMM yyyy")}";
            message.Body = body;

            using SmtpClient smtp = new SmtpClient(SmtpClientHost, SmtpClientPort);

            smtp.Send(message);

            Console.WriteLine("Email sent!");

        }
    }
}
