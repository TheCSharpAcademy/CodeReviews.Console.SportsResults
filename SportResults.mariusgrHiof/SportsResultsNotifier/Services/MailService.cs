using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace SportsResultsNotifier.Services;
public class MailService
{

    public MailService(string from, string to, string subject, string body)
    {
        _from = from;
        _to = to;
        _subject = subject;
        _body = body;
    }

    static ConfigurationManager _configurationManager = new ConfigurationManager();
    string smtpAddress = "smtp.gmail.com";
    int portNumber = 587;
    bool enableSSL = true;

    static string password = ""; // To get it to work with 2 factor Auth on gmail, 
                                 // I had to create "App password" and paste the password here.
    private readonly string _from;
    private readonly string _to;
    private readonly string _subject;
    private readonly string _body;

    public void SendEmail()
    {
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(_from);
            mail.To.Add(_to);
            mail.Subject = _subject;
            mail.Body = _body;
            mail.IsBodyHtml = true;
            //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
            using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
            {
                smtp.Credentials = new NetworkCredential(_from, password);
                smtp.EnableSsl = enableSSL;
                smtp.Send(mail);
            }
        }
    }
}

