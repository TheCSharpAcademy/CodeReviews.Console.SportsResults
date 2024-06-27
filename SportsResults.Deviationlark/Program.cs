using System.Net;
using System.Net.Mail;

namespace SportsResultsNotifier;

public class Program
{
    public static void Main(string[] args)
    {
        string url = "https://www.basketball-reference.com/boxscores/";
        var gameAmount = WebScraper.ScrapeGameAmmount(url);
        var date = WebScraper.ScrapeDate(url);
        var games = WebScraper.Scraper(url);
        string gameData = @$"
{gameAmount}
{date}
";
        int gameNum = 1;
        foreach (var game in games)
        {
            gameData += @$"
Game number:{gameNum}
{game.SportsTeam1}: {game.Team1Score}
{game.SportsTeam2}: {game.Team2Score}
            "; 
            gameNum++;
        }
        SendEmail(gameData);
    }

    public static void SendEmail(string gameData)
    {
        string smtpAddress = "smtp.gmail.com";
        int portNumber = 587;
        bool enableSSL = true;
        // enter your own testing emails and password
        string emailFromAddress = "";
        string password = "";
        string emailToAddress = "";
        string subject = "Sports Results NBA";
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(emailFromAddress);
            mail.To.Add(emailToAddress);
            mail.Subject = subject;
            mail.Body = gameData;
            mail.IsBodyHtml = true;
            using (SmtpClient smtpClient = new SmtpClient(smtpAddress, portNumber))
            {
                smtpClient.Credentials = new NetworkCredential(emailFromAddress, password);
                smtpClient.EnableSsl = enableSSL;
                smtpClient.Send(mail);
            }
        }
        Console.WriteLine("Email sent successfully!");
    }
}
