using Microsoft.Extensions.Configuration;
using SportsResultsNotifier.Arashi256.Models;

namespace SportsResultsNotifier.Arashi256.Config
{
    internal class AppManager
    {
        private readonly IConfiguration _configuration;

        public AppManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SMTPSettings GetSMTPSettings()
        {
            var smtpSettings = new SMTPSettings();
            smtpSettings.SmtpServer = _configuration["SMTPSettings:SMTPServer"];
            smtpSettings.SmtpPort = GetSMTPPort(_configuration["SMTPSettings:SMTPPort"]);
            smtpSettings.SmtpUsername = _configuration["SMTPSettings:SMTPUser"];
            smtpSettings.SmtpPassword = _configuration["SMTPSettings:SMTPPassword"];
            smtpSettings.SmtpSSLEnabled = GetSmtpUseSSL(_configuration["SMTPSettings:SmtpUseSSL"]);
            return smtpSettings;
        }

        public string? GetApiUrl()
        {
            return _configuration["API_Url:Url_Basketball"];
        }

        private int GetSMTPPort(string? strPort)
        {
            var portString = strPort;
            int port = 0;
            if (!int.TryParse(portString, out port))
            {
                port = 0;
            }
            return port;
        }

        private bool GetSmtpUseSSL(string? strUseSSL)
        {
            var useSSLString = strUseSSL;
            bool useSSL = false;
            if (!bool.TryParse(useSSLString, out useSSL))
            {
                useSSL = false;
            }
            return useSSL;
        }
    }
}