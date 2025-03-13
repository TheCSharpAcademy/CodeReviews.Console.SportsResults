using HtmlAgilityPack;
using Quartz;
using System.Net.Mail;
using System.Net;
using System.Text.Json;

namespace SportsResults.cacheMe512;

internal class ScraperJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Scraping started at {DateTime.Now}");
        string data = ScrapeBasketballData();
        SendEmail(data);
        Console.WriteLine("Email Sent!");
    }

    public string ScrapeBasketballData()
    {
        string url = "https://www.basketball-reference.com/";
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);

        var gameSummaries = doc.DocumentNode.SelectNodes("//div[contains(@class, 'game_summary')]");
        string scrapedData = "";

        if (gameSummaries != null)
        {
            foreach (var game in gameSummaries)
            {
                var teams = game.SelectNodes(".//table[contains(@class, 'teams')]//tr");
                if (teams != null)
                {
                    List<string> teamNames = new List<string>();
                    List<string> scores = new List<string>();

                    foreach (var row in teams)
                    {
                        var teamNode = row.SelectSingleNode(".//td/a");
                        var scoreNode = row.SelectSingleNode(".//td[@class='right'][1]");

                        if (teamNode != null && scoreNode != null)
                        {
                            teamNames.Add(teamNode.InnerText.Trim());
                            scores.Add(scoreNode.InnerText.Trim());
                        }
                    }

                    if (teamNames.Count == 2 && scores.Count == 2)
                    {
                        scrapedData += $"{teamNames[0]}: {scores[0]} - {teamNames[1]}: {scores[1]}\n";
                    }
                }
            }
        }
        return scrapedData;
    }

    public void SendEmail(string body)
    {
        var config = LoadConfiguration();

        MailMessage mail = new MailMessage(config.FromEmail, config.ToEmail)
        {
            Subject = "Daily Basketball Scores",
            Body = body,
            IsBodyHtml = false
        };

        SmtpClient client = new SmtpClient(config.SmtpServer, config.SmtpPort)
        {
            Credentials = new NetworkCredential(config.FromEmail, config.EmailPassword),
            EnableSsl = true
        };
        client.Send(mail);
    }

    private EmailConfig LoadConfiguration()
    {
        string configPath = "appsettings.json";
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Configuration file not found: " + configPath);
        }
        string json = File.ReadAllText(configPath);
        return JsonSerializer.Deserialize<EmailConfig>(json);
    }
}
