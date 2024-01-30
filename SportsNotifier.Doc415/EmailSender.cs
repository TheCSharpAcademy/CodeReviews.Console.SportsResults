using System.Net;
using System.Net.Mail;
namespace SportsNotifier;

internal class EmailSender
{
    string smtpServer = "smtp.gmail.com"; //enter your smtp server 
    string userName = "serdar415@gmail.com";   //enter your username
    string password = "ufzr ycxk tgmx ufdz";   //enter your password
    string subject;
    string body;
    string recipentEmail;
    string myEmail = "serdar415@gmail.com";    //enter your e-mail

    public void SendEmail(string _recipientEmail, string message)
    {
        subject = "Weekly NBA results";
        recipentEmail = _recipientEmail;
        body = message;
        var smtpClient = new SmtpClient(smtpServer)
        {
            Port = 587,
            EnableSsl = true,
        };
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(userName, password);

        try
        {
            smtpClient.Send(myEmail, recipentEmail, subject, body);

        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"There was an error sending mail: {ex.Message}");
        }
    }
}
