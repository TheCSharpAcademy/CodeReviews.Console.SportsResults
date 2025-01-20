using Microsoft.Extensions.Logging;
using SportsResultsNotifier.Arashi256.Classes;
using SportsResultsNotifier.Arashi256.Config;
using SportsResultsNotifier.Arashi256.Models;
using System.Net.Mail;


namespace SportsResultsNotifier.Arashi256.Services
{
    internal class EmailService
    {
        private AppManager _appManager;
        private readonly ILogger<EmailService> _logger;

        public EmailService(AppManager appManager, ILogger<EmailService> logger)
        {
            _logger = logger;
            _appManager = appManager;
        }

        private bool CheckValidSettings(SMTPSettings smtpSettings)
        {
            if (smtpSettings != null)
            {
                if ((smtpSettings.SmtpServer == null || smtpSettings.SmtpServer.Length == 0) ||
                    (smtpSettings.SmtpPort == 0))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
                return false;
        }

        private ServiceResponse InitClient()
        {
            SMTPSettings smtpSettings = _appManager.GetSMTPSettings();
            if (!CheckValidSettings(smtpSettings))
            {
                _logger.LogError("SMTP settings couldn't be read from appsettings.json");
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, "SMTP host settings invalid. Please check your configuration.", null);
            }
            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpSettings.SmtpServer);
                smtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(smtpSettings.SmtpUsername, smtpSettings.SmtpPassword);
                smtpClient.Credentials = basicAuthenticationInfo;
                smtpClient.Port = smtpSettings.SmtpPort;
                smtpClient.EnableSsl = smtpSettings.SmtpSSLEnabled;
                _logger.LogInformation("SMTP client settings initialised");
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK", smtpClient);
            }
            catch (Exception ex)
            {
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }

        public async Task<ServiceResponse> SendEmailAsync(Contact sender, Contact receiver, string subject, string? content)
        {
            try
            {
                var response = InitClient();
                if (response.Status.Equals(ResponseStatus.Success))
                {
                    var senderAddress = sender.Address ?? throw new ArgumentNullException(nameof(sender.Address), "Sender address cannot be null");
                    var senderName = sender.Name ?? throw new ArgumentNullException(nameof(sender.Name), "Sender name cannot be null");
                    var receiverAddress = receiver.Address ?? throw new ArgumentNullException(nameof(receiver.Address), "Receiver address cannot be null");
                    var receiverName = receiver.Name ?? throw new ArgumentNullException(nameof(receiver.Name), "Receiver name cannot be null");
                    MailAddress from = new MailAddress(senderAddress, senderName);
                    MailAddress to = new MailAddress(receiverAddress, receiverName);
                    MailMessage myMail = new MailMessage(from, to);
                    MailAddress replyTo = new MailAddress(sender.Address);
                    myMail.ReplyToList.Add(replyTo);
                    myMail.Subject = subject;
                    myMail.SubjectEncoding = System.Text.Encoding.UTF8;
                    myMail.Body = content;
                    myMail.BodyEncoding = System.Text.Encoding.UTF8;
                    myMail.IsBodyHtml = false;
                    var smtpClient = response.Data as SmtpClient;
                    if (smtpClient != null)
                    {
                        await smtpClient.SendMailAsync(myMail);
                        _logger.LogInformation("OK - email sent");
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Success, "OK - email sent", null);
                    }
                    else
                    {
                        string error = "SMTP client could not be cast from initialisation response";
                        _logger.LogError(error);
                        return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, error, null);
                    }
                }
                else
                {
                    _logger.LogError(response.Message);
                    return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, response.Message, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ServiceResponseUtils.CreateResponse(ResponseStatus.Failure, ex.Message, null);
            }
        }
    }
}