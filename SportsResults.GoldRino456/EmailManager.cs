using SportsResultsNotifier.Model;
using System.Net;
using System.Net.Mail;

namespace SportsResultsNotifier;

public static class EmailManager
{
    //SMTP Settings
    static string _smtpAddress = String.Empty;
    static int _smtpPort = 0;
    static bool _smtpEnableSsl = false;
    static string _senderEmail = String.Empty;
    static string _senderPassword = String.Empty;
    static string _destinationEmail = String.Empty;

    //Manager State
    static bool _isConfigLoaded = false;

    public static void SendEmail(EmailData emailData)
    {
        if (!_isConfigLoaded)
        {
            LoadSmtpConfiguration();
        }

        BuildAndSendEmail(emailData);
    }

    private static void BuildAndSendEmail(EmailData emailData)
    {
        using (MailMessage mail = new())
        {
            mail.From = new MailAddress(_senderEmail);
            mail.To.Add(_destinationEmail);

            mail.Subject = emailData.Subject;
            mail.Body = emailData.Body;
            mail.IsBodyHtml = false;

            using (SmtpClient smtp = new(_smtpAddress, _smtpPort))
            {
                smtp.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                smtp.EnableSsl = _smtpEnableSsl;
                smtp.Send(mail);
            }
        }
    }

    private static void LoadSmtpConfiguration()
    {
        var wasConfigReadSuccessfully = AppConfig.FetchSmtpSettings(out string? smtpAddress, out int? smtpPort,
                out bool? smtpEnableSsl, out string? senderEmail, out string? senderPassword, out string? destinationEmail);

        if (wasConfigReadSuccessfully)
        {
            _smtpAddress = smtpAddress;
            _smtpPort = smtpPort.Value;
            _smtpEnableSsl = smtpEnableSsl.Value;
            _senderEmail = senderEmail;
            _senderPassword = senderPassword;
            _destinationEmail = destinationEmail;

            _isConfigLoaded = true;
        }
        else
        {
            throw new Exception("Error loading configuration. Please ensure SMTP values are saved in appsettings.json.");
        }
    }
}
