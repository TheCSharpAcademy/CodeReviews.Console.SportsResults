using Microsoft.Extensions.Hosting;
using SportsResultsNotifier.Services;

public class DailyJobService : IHostedService, IDisposable
{
    private Timer _timer;

    private ScrapeSite scrapeSite = new ScrapeSite();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var desiredTime = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0); // Set the desired time (12:00 PM in this example)
        var delay = desiredTime > now ? desiredTime - now : TimeSpan.FromHours(24) - (now - desiredTime);

        _timer = new Timer(DoWork, null, delay, TimeSpan.FromHours(24));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        Console.WriteLine("Method executed once a day.");
        // Add your method logic here
        scrapeSite.GetData();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
