using System.Timers;
using Timer = System.Timers.Timer;

namespace SportsResults.Controllers;

public class AppController
{
    public EmailSender _emailSender;
    private readonly Scrapper _scrapper;
    private Timer _timer;

    public AppController(string urlToScrap, EmailSender emailSender)
    {
        _emailSender = emailSender;
        _scrapper = new Scrapper(urlToScrap);
        _timer = new Timer(TimeSpan.FromHours(24).TotalMilliseconds);
        _timer.Elapsed += Execute;
        _timer.AutoReset = true;
    }

    public void StartController()
    {
        _timer.Start();
        Execute(null, null);
    }

    private void Execute(object sender, ElapsedEventArgs e)
    {
        var matches = _scrapper.ScrapeMatches();
        var messageBody = MessageHelper.GetFormattedMessageBody(matches);
        _emailSender.Send(messageBody);
    }
}