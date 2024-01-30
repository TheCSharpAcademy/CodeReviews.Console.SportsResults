using Microsoft.Extensions.Hosting;
namespace SportsNotifier;

internal class ScoreResultsService : BackgroundService
{
    EmailSender _sender;
    MessageBuilder _messageBuilder;
    WebScrape _scrapper;
    List<string> _reciepents = new List<string>() { "foo@foo.com", "goo@goo.com", "basketenthusiasts@worldwide.web" };
    TimeSpan _interval;

    public ScoreResultsService()
    {
        _messageBuilder = new MessageBuilder();
        _scrapper = new WebScrape();
        _sender = new EmailSender();
        _interval = new TimeSpan(0, 0, 15);
    }

    protected override async Task ExecuteAsync(CancellationToken stopToken)
    {
        SetInterval(() =>
        {
            var data = _scrapper.GetGames();
            var message = _messageBuilder.BuildMessage(data);
            foreach (var reciepent in _reciepents)
                _sender.SendEmail(reciepent, message);

        }, _interval).Wait();
    }

    static async Task SetInterval(Action action, TimeSpan timeout)
    {
        await Task.Delay(timeout).ConfigureAwait(false);

        action();

        SetInterval(action, timeout);
    }
}

