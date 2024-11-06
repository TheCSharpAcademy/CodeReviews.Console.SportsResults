namespace SportsNotifier.hasona23.Config;

public static class AppSetting
{
    public static string BasketBallUrl { get; } = string.Empty;

    static AppSetting()
    {
    
        try
        {
            var config = new ConfigurationManager().SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json").Build();
            BasketBallUrl = config.GetSection("WebUrl:BasketBallUrl").Value ?? "Couldn't find url";

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //BasketBallUrl = path;
        }
    }
}