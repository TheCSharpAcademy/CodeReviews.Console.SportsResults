using System.Net;
using System.Net.Mail;

namespace Sports_Result_Notifier.Lawang;

public class MailSender
{

    public void SendEmail(string message, string receiver)
    {
        DotNetEnv.Env.Load();

        var senderMailAddress = new MailAddress(Environment.GetEnvironmentVariable("SenderEmail") ?? "");
        var appPassword = Environment.GetEnvironmentVariable("Password");
        SmtpClient mailClient = new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(senderMailAddress.Address, appPassword)
        };


        var toAddress = new MailAddress(receiver);
        using var mailMessage = new MailMessage(senderMailAddress, toAddress)
        {
            Subject = "Today's basket ball result",
            Body = message,
            IsBodyHtml = true
        };

        try
        {
            mailClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured while sending mail: {ex.Message}");
        }

    }
}
