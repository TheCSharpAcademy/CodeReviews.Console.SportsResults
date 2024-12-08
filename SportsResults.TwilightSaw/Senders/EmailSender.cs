using Microsoft.Extensions.Hosting;
using SportsResults.TwilightSaw.Builders;
using SportsResults.TwilightSaw.Controllers;

namespace SportsResults.TwilightSaw.Senders;

public class EmailSender(EmailController emailController, MessageBuilder messageBuilder) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stopToken)
    {
        var message = messageBuilder.GetGameMessage();
        //emailController.SendEmail("Name", "blackpanterastudio@gmail.com", message, "Today's NBA Games");
        Console.WriteLine(message);
    }
    
    static async Task SetInterval(Action action, TimeSpan timeout)
    {
        await Task.Delay(timeout).ConfigureAwait(false);
        action();
        SetInterval(action, timeout);
    }
}