using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace SportsResults.jollejonas.Services;
public class Mail
{
    private readonly string _smtpAddress;
    private readonly int _portNumber;
    private readonly bool _enableSSL;
    private readonly MailAddress _fromAddress;
    private readonly string _password;
    private readonly MailAddress _toAddress;

    public Mail(IConfiguration configuration)
    {
        _smtpAddress = "smtp.gmail.com";
        _portNumber = 587;
        _enableSSL = true;
        _fromAddress = new MailAddress(configuration["SmtpSettings:Username"]);
        _password = configuration["SmtpSettings:Password"];
        _toAddress = new MailAddress("jollevb@hotmail.com", "To Jonas");
    }

    public void SendMail(string message)
    {
        try
        {
            string subject = $"Daily Sports Results - {DateTime.Now.Date}";
            string body = message;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = _fromAddress;
                mail.To.Add(_toAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient(_smtpAddress, _portNumber))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(_fromAddress.Address, _password);
                    smtp.EnableSsl = _enableSSL;
                    smtp.Send(mail);
                }
            }
            Console.WriteLine("Email sent successfully.");
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"SMTP Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");
        }
    }

}
