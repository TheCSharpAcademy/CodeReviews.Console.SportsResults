using HtmlAgilityPack;

namespace SportsResults.KamilKolanowski.Services;

internal class WebScrapperService
{
    internal string ScrapWebsite(string url)
    {
        HtmlWeb web = new HtmlWeb();
        var htmlDoc = web.Load(url);
        var node = htmlDoc.DocumentNode.SelectSingleNode(
            "//div[@id='content']//div[contains(@class, 'game_summary') and contains(@class, 'expanded') and contains(@class, 'nohover')]"
        );

        if (node == null)
            return "<html><body><p>No game summary found.</p></body></html>";

        string htmlBody = node.OuterHtml;

        var emailHtml =
            $@"
        <html>
        <head>
          <meta charset='UTF-8'>
          <style>
            table {{ border-collapse: collapse; width: 100%; }}
            th, td {{ border: 1px solid #ccc; padding: 8px; text-align: center; }}
            th {{ background-color: #f4f4f4; }}
          </style>
        </head>
        <body>
          <h2>Basketball Box Score</h2>
          {htmlBody}
        </body>
        </html>";

        return emailHtml;
    }
}
