namespace SportsResults.Utilities;
public class ConfigReader
{
    private readonly IConfiguration _configuration;

    public ConfigReader(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetWebsiteUrl()
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

public class EmailSettings
{
    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string FromAddress { get; set; } = "";
    public string ToAddress { get; set; } = "";
}