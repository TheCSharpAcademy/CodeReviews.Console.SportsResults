using Microsoft.Extensions.Configuration;

namespace SportsResults;

public class ConfigurationService
{
    public string? SmtpAddress { set; get; }
    public int PortNumber { set; get; }
    public bool EnableSSL { set; get; }
    public string? EmailFromAddress { set; get; }
    public string? Password { set; get; }
    public string? EmailToAddress { set; get; }
    private IConfiguration _configuration;

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void GetConfiguration()
    {
        SmtpAddress = _configuration.GetSection("Configuration:smtpAddress").Get<string>();
        PortNumber = _configuration.GetSection("Configuration:portNumber").Get<int>();
        EnableSSL = _configuration.GetSection("Configuration:enableSSL").Get<bool>();
        EmailFromAddress = _configuration.GetSection("Configuration:emailFromAddress").Get<string>();
        Password = _configuration.GetSection("Configuration:password").Get<string>();
        EmailToAddress = _configuration.GetSection("Configuration:emailToAddress").Get<string>();
    }
}