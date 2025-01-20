using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Net.Mail;

namespace SportsResults;

public class EmailSender
{
    public string Body { get; set; }
    public EmailSender(string body)
    {
        Body = body;
    }

    public void Send()
    {
        var sender = new SmtpSender(new SmtpClient(GlobalConfig.SmtpClient)
        {
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 25,
        });
        Email.DefaultSender = sender;

        try
        {
            var email = Email
                .From(GlobalConfig.MailFrom)
                .To(GlobalConfig.MailTo, "SportsResultsUser")
                .Subject($"Results Date: {DateTime.Today:yyyy-MM-dd}")
                .Body(Body)
                .Send();
            Console.WriteLine(email.ErrorMessages);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}