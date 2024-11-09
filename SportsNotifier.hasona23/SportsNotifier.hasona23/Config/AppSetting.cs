namespace SportsNotifier.hasona23.Config;

public static class AppSetting
{
    public static string BasketBallUrl { get; } = string.Empty;
    public static string SmtpHost { get; } = string.Empty;
    public static int SmtpPort { get; } = 0;
    //Username is also the email of the sender
    public static string SmtpUsername { get; } = string.Empty;
    public static string SmtpPassword { get; } = string.Empty;
    static AppSetting()
    {
    
        try
        {
            var config = new ConfigurationManager().SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json").Build();
            BasketBallUrl = config.GetSection("WebUrl:BasketBallUrl").Value ?? "Couldn't find url";
            SmtpHost = config["Smtp:Host"] ?? "smtp.gmail.com";
            SmtpPassword = config["Smtp:Password"] ?? "PASSWORD";
            SmtpPort = int.Parse(config["Smtp:Port"] ?? "587");
            SmtpUsername = config["Smtp:Username"]??"USERNAME";

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //BasketBallUrl = path;
        }
    }
}