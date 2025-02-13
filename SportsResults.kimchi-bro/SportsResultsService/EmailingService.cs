using HtmlAgilityPack;
using MimeKit;
using MailKit.Net.Smtp;

namespace SportsResultsService;

public sealed class EmailingService(IConfiguration configuration)
{
    public void SendEmail()
    {
        var web = new HtmlWeb();

        var htmlDoc = web.Load("https://www.basketball-reference.com/boxscores/");

        var title = htmlDoc.DocumentNode.SelectSingleNode("//h1");
        var tables = htmlDoc.DocumentNode.SelectNodes("//div[@class='game_summaries']//table[@class='teams']");

        var results = GetResults(tables);

        var emailMessage = CreateMessage(configuration, title, results);

        Send(configuration, emailMessage);
    }

    private static List<string> GetResults(HtmlNodeCollection tables)
    {
        var results = new List<string>();
        for (int i = 0; i < tables.Count; i++)
        {
            var teamNames = new List<string>();
            var teamScores = new List<string>();

            var teamNodes = tables[i].SelectNodes(".//tr");
            foreach (var teamNode in teamNodes)
            {
                var teamName = teamNode.SelectSingleNode(".//a").InnerText;
                var teamScore = teamNode.SelectSingleNode(".//td[@class='right'][1]").InnerText;

                teamNames.Add(teamName);
                teamScores.Add(teamScore);
            }

            results.Add($"{teamNames[0]} {teamScores[0]}, {teamNames[1]} {teamScores[1]}");
        }

        return results;
    }

    private static MimeMessage CreateMessage(
        IConfiguration configuration, HtmlNode title, List<string> results)
    {
        var emailBodyText = "";
        foreach (var result in results)
        {
            emailBodyText += $"{result}\n";
        }

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Sports Results", configuration["Smtp:EmailFrom"]));
        emailMessage.To.Add(new MailboxAddress("Sports Results", configuration["Smtp:EmailTo"]));
        emailMessage.Subject = title.InnerHtml;
        emailMessage.Body = new TextPart("plain")
        {
            Text = $"*{title.InnerHtml}*\n\n" + emailBodyText
        };

        return emailMessage;
    }

    private static void Send(IConfiguration configuration, MimeMessage emailMessage)
    {
        using var smtpClient = new SmtpClient();
        smtpClient.Connect(configuration["Smtp:Server"], configuration.GetValue<int>("Smtp:Port"));
        smtpClient.Authenticate(configuration["Smtp:EmailFrom"], configuration["Smtp:Password"]);
        smtpClient.Send(emailMessage);
        smtpClient.Disconnect(true);
    }
}
