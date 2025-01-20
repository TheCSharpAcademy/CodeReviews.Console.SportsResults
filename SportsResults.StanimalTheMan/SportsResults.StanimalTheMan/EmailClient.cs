using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SportsResults.StanimalTheMan;

internal static class EmailClient
{
    static string smtpAddress = "smtp.gmail.com";
    static int portNumber = 587;
    static bool enableSSL = true;
    static string emailFromAddress = "stanjychoi@gmail.com"; //Sender Email Address  
    static string password = ConfigurationManager.AppSettings["Password"]; //Sender Password  
    static string emailToAddress = "stanjychoi@gmail.com"; //Receiver Email Address  
    static string subject = "Most Recent Daily NBA Scores";
    static string body;
    public static void SendEmail(List<Game> gamesData)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{gamesData[0].Date}<br>");
        foreach(Game game in gamesData)
        {
            stringBuilder.AppendLine($"Winner: {game.WinningTeam} {game.WinningTeamPoints}, Loser: {game.LosingTeam} {game.LosingTeamPoints}<br>");
        }
        body = stringBuilder.ToString();
        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(emailFromAddress);
            mail.To.Add(emailToAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
            using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
            {
                smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                smtp.EnableSsl = enableSSL;
                smtp.Send(mail);
            }
        }
    }
}
