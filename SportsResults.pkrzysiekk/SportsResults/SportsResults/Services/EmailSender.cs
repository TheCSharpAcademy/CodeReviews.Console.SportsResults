using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace SportsResults;

public class EmailSender
{
    private string _smtpAddress;
    private int _smtpPort;
    private bool _smtpEnableSsl = true;
    private string _smtpUsername;
    private string _smtpPassword;
    private List<MailAddress> _mailTo = new List<MailAddress>();
    private SmtpClient _smtpClient;


    public EmailSender()
    {
        using (var config = new ConfigurationManager())
        {
            try
            {
                config.AddJsonFile("appsettings.json");
                _smtpAddress = config["Email:SmtpAddress"];
                _smtpPort = int.Parse(config["Email:SmtpPort"]);
                _smtpUsername = config["Email:SenderName"];
                _smtpPassword = config["Email:SenderPassword"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        _smtpClient = new SmtpClient(_smtpAddress, _smtpPort);
        _smtpClient.Credentials=new NetworkCredential(_smtpUsername, _smtpPassword);
        _smtpClient.EnableSsl = _smtpEnableSsl;
       
    }
    
    public void AddReceiver(string email)
    {
        _mailTo.Add(new MailAddress(email));
    }

    private MailMessage CreateMessageTemplate(MailAddress receiver)
    {
        var message = new MailMessage();
        message.From = new MailAddress(_smtpUsername);
        message.To.Add(receiver);
        return message;
        
    }
    public void Send(string body, string subject= "Basketball info")
    {
        foreach (var mail in _mailTo)
        {
            var messageTemplate= CreateMessageTemplate(mail);
            messageTemplate.Subject = subject;
            messageTemplate.Body = body;
            _smtpClient.Send(messageTemplate);
        }
    }


}