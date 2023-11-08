using System.Net;
using System.Net.Mail;
using SportsResults.wkktoria.Models;

namespace SportsResults.wkktoria;

public static class Mailer
{
    private static readonly List<Game> Games = Scraper.GetGamesData();

    private static string CreateEmailBody(IEnumerable<Game> games)
    {
        return games.Select(game => $"""
                                     <div>
                                       <h3>{game.Winner} vs. {game.Loser}</h3>
                                       <p>{game.Winner}: <b>{game.WinnerScore}</b></p>
                                       <p>{game.Loser}: <b>{game.LoserScore}</b></p>
                                     </div>
                                     """)
            .Aggregate("", (current, gameDiv) => current + gameDiv);
    }

    public static void SendEmail(string senderEmail, string senderPassword, string receiverEmail)
    {
        using var mailMessage = new MailMessage();

        mailMessage.From = new MailAddress(senderEmail);
        mailMessage.To.Add(receiverEmail);
        mailMessage.Subject = "Today's Sports Results";
        mailMessage.Body = CreateEmailBody(Games);
        mailMessage.IsBodyHtml = true;

        using var client = new SmtpClient("smtp.gmail.com", 587);

        client.Credentials = new NetworkCredential(senderEmail, senderPassword);
        client.EnableSsl = true;
        client.Send(mailMessage);
    }
}