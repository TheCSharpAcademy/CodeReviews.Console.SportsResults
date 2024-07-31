using System.Net;
using System.Net.Mail;
using SportsResults.Mail.Models;

namespace SportsResults.Mail.Services;

/// <summary>
/// Responsible for configuring the SMTP client and sending the Email Message.
/// </summary>
public class EmailService
{
    #region Fields

    private readonly string _smtpClientHost;
    private readonly int _smtpClientPort;
    private readonly NetworkCredential _smtpClientNetworkCredential;
    private readonly bool _smtpClientEnableSsl;

    #endregion
    #region Constructors

    public EmailService(string smtpClientHost, int smtpClientPort, NetworkCredential smtpClientNetworkCredential, bool smtpClientEnableSsl)
    {
        _smtpClientHost = smtpClientHost;
        _smtpClientPort = smtpClientPort;
        _smtpClientNetworkCredential = smtpClientNetworkCredential;
        _smtpClientEnableSsl = smtpClientEnableSsl;
    }

    #endregion
    #region Methods

    public void SendEmail(EmailMessage emailMessage)
    {
        using var mailMessage = emailMessage.ToMailMessage();

        using var client = new SmtpClient(_smtpClientHost, _smtpClientPort);
        client.Credentials = _smtpClientNetworkCredential;
        client.EnableSsl = _smtpClientEnableSsl;
        client.Send(mailMessage);
    }

    #endregion
}
