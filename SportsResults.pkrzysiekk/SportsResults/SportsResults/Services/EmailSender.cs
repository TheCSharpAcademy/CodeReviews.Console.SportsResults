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
    private List<string> _mailTo;
    private SmtpClient _smtpClient;


    public EmailSender()
    {
        using (var config = new ConfigurationManager())
        {
            
        }
    }
    
    public void AddReceiver(string email)
    {
        _mailTo.Add(email);
    }



}