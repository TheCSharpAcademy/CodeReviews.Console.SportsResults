using SportsNotifier.hasona23.Config;

namespace SportsNotifier.hasona23.Scrapper;

public class Scrapper
{
    public string Url { get; set; }
    public Scrapper()
    {
        Url = AppSetting.BasketBallUrl;
    }
    
}