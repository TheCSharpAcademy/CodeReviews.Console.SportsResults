using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace SportsResults.K_MYR;

public class MailingService : BackgroundService
{
    private readonly ILogger<MailingService> _Logger;
    private readonly IWebScrapper _Scrapper;
    private readonly string StmpAdress = "smtp.gmail.com";
    private readonly int PortNumber = 587;
    private readonly bool EnableSSL = true;
    private readonly string EmailSender = ConfigurationManager.AppSettings.Get("EmailSender") ?? "";
    private readonly string Password = ConfigurationManager.AppSettings.Get("EmailPassword") ?? "";
    private readonly string EmailReceiver = ConfigurationManager.AppSettings.Get("EmailReceiver") ?? "";

    public MailingService(IWebScrapper scrapper, ILogger<MailingService> logger)
    {
        _Logger = logger;
        _Scrapper = scrapper;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        _Logger.LogInformation("Service is starting");

        SendMail();

        using PeriodicTimer timer = new(TimeSpan.FromDays(1));

        try
        {
            while (await timer.WaitForNextTickAsync(token))
            {
                SendMail();
            }
        }
        catch (OperationCanceledException)
        {
            _Logger.LogInformation("Service is stopping");
        }
    }

    private void SendMail()
    {
        try
        {
            var games = _Scrapper.GetGames();

            if (games.Count == 0)
            {
                _Logger.LogInformation("No New Games Were Found On {date}", DateTime.UtcNow);
                return;
            }

            using (MailMessage mail = new())
            {
                mail.From = new MailAddress(EmailSender);
                mail.To.Add(EmailReceiver);
                mail.Subject = $"Your Sports Update From {games[0].Date}";
                mail.Body = GenerateBody(games);
                mail.IsBodyHtml = true;

                using SmtpClient smtp = new(StmpAdress, PortNumber);
                smtp.Credentials = new NetworkCredential(EmailSender, Password);
                smtp.EnableSsl = EnableSSL;
                smtp.Send(mail);
            }

            _Logger.LogInformation("Email was send at {date}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _Logger.LogError("An Error Occured Sending The Mail: {ex}", ex.Message);
        }
    }

    private static string GenerateBody(List<GameSummary> games)
    {
        string body = @$"<h2>Welcome To Your Sports Update!</h2>
        <p>There were {games.Count} games played on {games[0].Date}:<p><hr/>";

        foreach (var game in games)
        {
            body += @$"
                        <div>
                            <h3><a href='{game.Gamelink}'>{game.GuestTeam} vs {game.HomeTeam}</a></h3>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>{game.GuestTeam}</td><td>{game.GuestTeamPoints}</td>
                                        </tr>
                                        <tr>
                                            <td>{game.HomeTeam}</td><td>{game.HomeTeamPoints}</td>
                                        </tr>
                                    </tbody>             
                            </table>
                            <table>
                                <thead><tr><th></th><th>1</th><th>2</th><th>3</th><th>4<th></thead>
                                <tbody>
                                    <tr>
                                        <td>{game.GuestTeam}</td>
                                        <td>{game.PointsPerQuarterGuest[0]}</td>
                                        <td>{game.PointsPerQuarterGuest[1]}</td>
                                        <td>{game.PointsPerQuarterGuest[2]}</td>
                                        <td>{game.PointsPerQuarterGuest[3]}</td>
                                    </tr>
                                    <tr>
                                        <td>{game.HomeTeam}</td>
                                        <td>{game.PointsPerQuarterHome[0]}</td>
                                        <td>{game.PointsPerQuarterHome[1]}</td>
                                        <td>{game.PointsPerQuarterHome[2]}</td>
                                        <td>{game.PointsPerQuarterHome[3]}</td>
                                    </tr>
                                </tbody>
                            </table>
                            <table>
                                <tbody>
                                    <tr>
                                        <td><strong>PTS</strong></td><td>{game.PlayerWithMostPoints}</td><td>{game.MostPoints}<td>
                                    </tr>
                                    <tr>

                                        <td><strong>TRB</strong></td><td>{game.PlayerWithMostTotalRebounds}</td><td>{game.MostTotalRebounds}<td>
                                    </tr>
                                </tbody>             
                            </table>
                            <hr/>
                        </div>";
        }

        return body;
    }
}
