using System.Net.Mail;

namespace SportsResults.Mail.Models;

/// <summary>
/// Represents an email message.
/// </summary>
public class EmailMessage
{
    #region Constructors

    public EmailMessage(string fromAddress, string toAddresses, string subject, EmailBody body)
    {
        ArgumentNullException.ThrowIfNull(fromAddress);
        ArgumentNullException.ThrowIfNull(toAddresses);
        ArgumentNullException.ThrowIfNull(subject);
        ArgumentNullException.ThrowIfNull(body);

        ArgumentException.ThrowIfNullOrWhiteSpace(fromAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(toAddresses);

        FromAddress = new EmailAddress(fromAddress);
        ToAddresses.Add(toAddresses);
        Subject = subject;
        Body = body.Text;
        IsBodyHtml = body.IsHtml;
    }

    #endregion
    #region Properties
    
    public EmailAddress FromAddress { get; set; }
        
    public EmailAddressCollection ToAddresses { get; set; } = [];

    public string Subject { get; set; }

    public string Body { get; set; }

    public bool IsBodyHtml { get; set; }

    #endregion
    #region Methods

    public MailMessage ToMailMessage()
    {
        var mailMessage = new MailMessage
        {
            From = FromAddress.ToMailAddress()
        };

        foreach (var address in ToAddresses)
        {
            mailMessage.To.Add(address.ToMailAddress());
        }

        mailMessage.Subject = Subject;

        mailMessage.Body = Body;

        mailMessage.IsBodyHtml = IsBodyHtml;

        return mailMessage;
    }

    #endregion
}
