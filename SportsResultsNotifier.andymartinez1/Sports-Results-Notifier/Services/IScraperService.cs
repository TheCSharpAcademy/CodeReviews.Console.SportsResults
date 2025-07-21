using HtmlAgilityPack;
using Sports_Results_Notifier.Models;

namespace Sports_Results_Notifier.Services;

public interface IScraperService
{
    public HtmlDocument ScrapeHtml(string html);

    public Game GetGamePlayedInfo(HtmlDocument doc);
}
