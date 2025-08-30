
namespace SportsResultsNotifier;

public static class AppConfig
{
    public static bool FetchSmtpSettings(out string? smtpAddress, out int? smtpPort,
        out bool? smtpEnableSsl, out string? senderEmail, out string? senderPassword, out string? destinationEmail)
    {
        string workingDirectory = AppContext.BaseDirectory;

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(workingDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var smtpSettings = config.GetSection("SmtpSettings");

        if (!smtpSettings.Exists())
        {
            smtpAddress = null;
            smtpPort = null;
            smtpEnableSsl = null;
            senderEmail = null;
            senderPassword = null;
            destinationEmail = null;
            return false;
        }

        smtpAddress = smtpSettings.GetValue<string>("smtpAddress");
        smtpPort = smtpSettings.GetValue<int>("portNumber");
        smtpEnableSsl = smtpSettings.GetValue<bool>("enableSsl");
        senderEmail = smtpSettings.GetValue<string>("senderEmail");
        senderPassword = smtpSettings.GetValue<string>("senderPassword");
        destinationEmail = smtpSettings.GetValue<string>("destinationEmail");

        if (smtpAddress == null || smtpPort == 0 || senderEmail == null
            || senderPassword == null || destinationEmail == null)
        {
            return false;
        }

        return true;
    }
}
