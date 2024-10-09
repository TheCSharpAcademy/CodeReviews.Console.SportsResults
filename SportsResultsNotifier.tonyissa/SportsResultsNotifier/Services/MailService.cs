using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Configuration;

namespace SportsResultsNotifier.Services;

public class MailService
{
    private readonly ILogger<MailService> _logger;
    private readonly string _sender;
    private readonly string _password;
    private readonly string _recipient;

    public MailService(IConfiguration config, ILogger<MailService> logger)
    {
        _logger = logger;

        var section = config.GetSection("MailConfiguration");
        _sender = section["SenderEmail"];
        _password = section["SenderPassword"];
        _recipient = section["RecipientEmail"];

        if (string.IsNullOrEmpty(_sender) || string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_recipient))
        {
            var ex = new ArgumentNullException(nameof(config), "Config is invalid. Please edit appsettings.json with desired mail config.");
            _logger.LogError(ex, "ArgumentNullException. Argument: {config} Reason: {message}", ex.ParamName, ex.Message);
            throw ex;
        }
    }
}