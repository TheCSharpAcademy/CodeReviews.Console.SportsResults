using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SportsResultsNotifier.Interfaces;
using System.Net;
using System.Net.Mail;

namespace SportsResultsNotifier.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string? _senderEmail;
    private readonly string? _address;
    private readonly int _smtpPort;
    private readonly string? _senderPassword;
    private readonly bool _enableSsl;

    private readonly string? _localHostAddress;
    private readonly int _localHostport;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;

        _senderEmail = _configuration.GetValue<string>("EmailSettings:SenderEmail");
        _address = _configuration.GetValue<string>("EmailSettings:Address");
        _smtpPort = _configuration.GetValue<int>("EmailSettings:Port");
        _senderPassword = _configuration.GetValue<string>("EmailSettings:SenderPassword");
        _enableSsl = _configuration.GetValue<bool>("EmailSettings:EnableSsl");

        _localHostAddress = _configuration.GetValue<string>("LocalHost:Address");
        _localHostport = _configuration.GetValue<int>("LocalHost:Port");
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        

        using MailMessage mail = new MailMessage();

        mail.From = new MailAddress(_senderEmail); 
        mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        using SmtpClient smtp = new SmtpClient(_address, _smtpPort) 
        {
            Credentials = new NetworkCredential(_senderEmail, _senderPassword),
            EnableSsl = _enableSsl
        };

        await smtp.SendMailAsync(mail);
    }

    public async Task SendLocalEmailAsync(string to, string subject, string body)
    {
        using MailMessage mail = new MailMessage();
        mail.From = new MailAddress(_senderEmail);
        mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        using SmtpClient smtp = new SmtpClient(_localHostAddress, _localHostport);
        await smtp.SendMailAsync(mail);
    }
}