using SportsResults.ukpagrace.Model;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using SportsResults.ukpagrace.Interfaces;
namespace SportsResults.ukpagrace.Mail
{
    public class SendEmail: IEmailInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IGameScraper _gameScraper;
        public SendEmail(IConfiguration configuration, IGameScraper gameScraper)
        {
            _configuration = configuration;
            _gameScraper = gameScraper;
        }
        public void SendMail()
        {
            var games = _gameScraper.ScrapGames(_configuration["Settings:Url"] ?? string.Empty);
            using MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration["Mail:From"]?? string.Empty);
            message.To.Add(_configuration["Mail:To"] ?? string.Empty);
            message.Subject = "Daily Baseball Game";
            message.IsBodyHtml = true;
            message.Body =  games.Count == 0 ? "No games were played" : EmailTemplate(games);

            using SmtpClient smtpClient = new SmtpClient(_configuration["Mail:Host"]);
            NetworkCredential networkCredential = new NetworkCredential(_configuration["Mail:Email"], _configuration["Mail:Password"]);
            smtpClient.Credentials = networkCredential;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            smtpClient.Send(message);
        }


        public string EmailTemplate(List<Game> games)
        {
            string htmlIntro = @$"
                <html lang=""en"">
                    <body>
                        <h1>Your Daily Games {DateTime.Now.ToShortTimeString()}</h1>
                           </br>";

            string tableIntro = $@"
                        <table>

                          <tr>
                            <th>Winner</th>
                            <th>Winner Score</th>
                            <th>Loser</th>
                            <th>Loser Score</th>
                          </tr>
            ";
            string tableContent = "";
            foreach(var game in games)
            {
                tableContent += @$"
                    <tr>
                        <th>{game.Winner}</th>
                        <th>{game.WinnerScore}</th>
                        <th>{game.Loser}</th>
                        <th>{game.LoserScore}</th>
                    </tr>
                ";
            };

            string tableEnd = "</table>";

            string htmlEnd = @"
                    </body>
                </html>
            ";

            string template = htmlIntro + tableIntro + tableContent + tableEnd + htmlEnd;

            return template;
        }
    }
}
