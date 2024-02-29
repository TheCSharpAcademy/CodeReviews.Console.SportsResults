using SportsResultsNotifier.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SportsResultsNotifier.UI;

namespace SportsResultsNotifier;

public class SportsResultsNotifier
{
    public static async Task Main()
    {
        IHost? app;
        DataController controller;
        try
        {
            app = StartUp.AppInit();
            controller = app.Services.CreateScope()
            .ServiceProvider.GetRequiredService<DataController>();
        }
        catch(Exception e)
        {
            MainUI.ErrorMessage(e.Message);
            Thread.Sleep(4000);
            return;
        }

        await controller.Start();
        MainUI.ExitMessage();
    }
}
