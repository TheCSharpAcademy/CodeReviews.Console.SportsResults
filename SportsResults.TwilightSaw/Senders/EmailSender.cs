using Microsoft.Extensions.Hosting;
using SportsResults.TwilightSaw.Builders;
using SportsResults.TwilightSaw.Controllers;

namespace SportsResults.TwilightSaw.Senders;

public class EmailSender(EmailController emailController, MessageBuilder messageBuilder) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stopToken)
    {
        SetInterval(() =>
        {
            var message = "Hello, there is your fresh news about the recent basketball matches:\n\n" + messageBuilder.GetGameMessage() + "\n" + messageBuilder.GetGameStatisticsMessage();
            emailController.SendEmail("Name", "blackpanterastudio@gmail.com", message, "Today's NBA Games");
        }, new TimeSpan(24,0,0)).Wait();
    }

    static async Task SetInterval(Action action, TimeSpan timeout)
    {
        await Task.Delay(timeout).ConfigureAwait(false);
        action();
        SetInterval(action, timeout);
    }
}