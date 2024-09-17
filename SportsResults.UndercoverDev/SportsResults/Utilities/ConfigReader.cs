using SportsResults.Models;

namespace SportsResults.Utilities;
public class ConfigReader
{
    private readonly IConfiguration _configuration;

    public ConfigReader(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string? GetWebsiteUrl()
    {
        return _configuration["Scraping:WebsiteUrl"];
    }
    public EmailSettings GetEmailSettings()
    {
        return new EmailSettings
        {
            SmtpServer = _configuration["Email:SmtpServer"],
            SmtpPort = int.Parse(_configuration["Email:SmtpPort"]),
            Username = _configuration["Email:Username"],
            Password = _configuration["Email:Password"],
            FromAddress = _configuration["Email:FromAddress"],
            ToAddress = _configuration["Email:ToAddress"]
        };
    }
}

