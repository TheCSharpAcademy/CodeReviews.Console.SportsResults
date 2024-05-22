using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace SportsResults.samggannon;

internal class EmailClient
{
    private readonly string? _SmtpAddress;
    private readonly string? _SmtpPassword;
    private readonly int _SmtpPort;
    private readonly string? _FromAddress;
    private readonly string? _ToAddress;
    private readonly string? _Subject;
    private readonly string? _Body;
    private readonly bool _EnableSSL = true;

    public EmailClient()
    {
    }

    public EmailClient(EmailSettings settings, string subject, string body, bool enableSSL = true)
    {
        _SmtpAddress = settings.SmtpAddress;
        _SmtpPassword = settings.SmtpPassword;
        _SmtpPort = settings.SmtpPort;
        _FromAddress = settings.FromAddress;
        _ToAddress = settings.ToAddress;
        _Subject = subject;
        _Body = body;
        _EnableSSL = enableSSL;
    }

    public void SendEmail()
    {
        var messages = Helpers.CheckEmailSettings(_SmtpAddress, _SmtpPassword, _SmtpPort, _FromAddress, _ToAddress);

        try
        {
            using (MailMessage mail = new())
            {
                mail.From = new MailAddress(_FromAddress);
                mail.To.Add(new MailAddress(_ToAddress));
                mail.Subject = _Subject;
                mail.Body = _Body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new(_SmtpAddress, _SmtpPort))
                {
                    smtp.Credentials = new NetworkCredential(_FromAddress, _SmtpPassword);
                    smtp.EnableSsl = _EnableSSL;
                    smtp.Send(mail);
                }
            }
        }
        catch (Exception ex)
        {
            // log exception
            // log not implemented
            Console.WriteLine(ex.ToString());
            Console.WriteLine();

            foreach(var message in messages)
            {
                Console.WriteLine(message.ToString());
            }

            Console.WriteLine("Terminating program");
            Environment.Exit(1);
        }
    }
}
