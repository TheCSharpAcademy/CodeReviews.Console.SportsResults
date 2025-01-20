namespace SportsResults;

internal class Startup
{
    public static IHost ConfigApplication()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration config = builder.Build();

        GlobalConfig.Url = config.GetValue<string>("Url")
            ?? throw new InvalidOperationException("You must provide a URL in the appsettings.json file.");
        GlobalConfig.SmtpClient = config.GetValue<string>("SmtpClient")
            ?? throw new InvalidOperationException("You must provide a SmtpClient in the appsettings.json file.");
        GlobalConfig.MailFrom = config.GetValue<string>("MailFrom")
            ?? throw new InvalidOperationException("You must provide a MailFrom in the appsettings.json file.");
        GlobalConfig.MailTo = config.GetValue<string>("MailTo")
            ?? throw new InvalidOperationException("You must provide a MailTo in the appsettings.json file.");

        var hostBuilder = Host.CreateApplicationBuilder();
        hostBuilder.Services.AddHostedService<Worker>();

        var host = hostBuilder.Build();

        return host;
    }
}