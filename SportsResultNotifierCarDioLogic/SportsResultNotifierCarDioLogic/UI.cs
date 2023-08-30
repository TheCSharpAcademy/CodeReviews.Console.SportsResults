using Spectre.Console;
using SportsResultNotifierCarDioLogic.Helpers;

namespace SportsResultNotifierCarDioLogic;

internal class UI
{
    EmailService emailService = new EmailService();
    internal void Menu()
    {
        bool isAppRunning = true;
        bool isAutomaticEmailsOn;

        do
        {
            string currentUser = NBAconfigHelpers.ReadFromNBAconfig().email;

            Console.Clear();

            Console.WriteLine($@"NBA results notifier!

Description:
This app will notify you everyday of all the NBA game results you need to know!
You need to SetUserConfigs to receive the results everyday by email!

Current user email: {currentUser}
");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                .Title("What would you like to do?")
                .AddChoices(
                MenuOptions.SetUserConfigs,
                MenuOptions.ClearUserConfigs,
                MenuOptions.SendEmail,
                MenuOptions.AutomaticEmail,
                MenuOptions.Exit));

            switch (option)
            {
                case MenuOptions.SetUserConfigs:
                    UILogic.SetEmailConfigs();
                    break;

                case MenuOptions.ClearUserConfigs:
                    if (NBAconfigHelpers.ReadFromNBAconfig().isAutomaticEmailsOn != true)
                    {
                        UILogic.DeleteEmailConfigs();
                        Console.WriteLine("Email user configs were deleted!!");
                    }
                    else
                    {
                        Console.WriteLine("You can't clear user configs while automatic emailer feature is on! Deactivate it first!");
                    }

                    Console.ReadLine();
                    break;

                case MenuOptions.SendEmail:
                    if (NBAconfigHelpers.ReadFromNBAconfig().email != "")
                    {
                        string date = AnsiConsole.Prompt(new TextPrompt<string>("Type date to get list of game results sent to Email (yyyy/MM/dd):"));
                        if (UILogic.IsValidDate(date))
                        {
                            (string subject, string body) = emailService.PrepareContent(date);
                            emailService.SendEmailService(subject, body);
                            Console.WriteLine("Results sent to email!");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Invalid date!");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have to define valid Email configs before activating the automatic emailer!");
                        Console.ReadLine();
                    }
                    break;

                case MenuOptions.AutomaticEmail:
                    if(NBAconfigHelpers.ReadFromNBAconfig().email != "")
                    {
                        AutomaticEmailMenu();
                    }
                    else
                    {
                        Console.WriteLine("You have to define valid Email configs before activating the automatic emailer!");
                        Console.ReadLine();
                    }
                    break;

                case MenuOptions.Exit:
                    isAppRunning = false;
                    break;
            }
        } while (isAppRunning == true);
    }

    public void AutomaticEmailMenu()
    {
        (string email, string decryptedPassword, bool isAutomaticEmailsOn) = NBAconfigHelpers.ReadFromNBAconfig();
        Console.WriteLine($"Automatic email is set to: {isAutomaticEmailsOn} \n");

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<AutomaticEmailActivator>()
            .Title("What would you like to do?")
            .AddChoices(
            AutomaticEmailActivator.ActivateAutomaticEmail,
            AutomaticEmailActivator.DeactivateAutomaticEmail));

        switch(option)
        {
            case AutomaticEmailActivator.ActivateAutomaticEmail:
                if(isAutomaticEmailsOn == false)
                {
                    WindowsTaskScheduler.CreateTask();
                    Console.WriteLine(@"Automatic Email activated! NBA game results of the previous days will be sent everyday at 12pm!
You can Deactivate automatic emails trough this App or by going to Windows Task Scheduler!");
                    isAutomaticEmailsOn = true;
                    NBAconfigHelpers.WriteToNBAconfig(email, decryptedPassword, isAutomaticEmailsOn);
                }
                else
                {
                    Console.WriteLine("Automatic emails feature is already actived!");
                }

                Console.ReadLine();
                break;

            case AutomaticEmailActivator.DeactivateAutomaticEmail:
                if(isAutomaticEmailsOn == true)
                {
                    WindowsTaskScheduler.DeleteTask();
                    isAutomaticEmailsOn = false;
                    NBAconfigHelpers.WriteToNBAconfig(email, decryptedPassword, isAutomaticEmailsOn);
                    Console.WriteLine("Automatic Email feature was deactivacted!");

                }
                else
                {
                    Console.WriteLine("Automatic emails feature is already Deactivated!");
                }

                Console.ReadLine();
                break;
        }
    }

    internal enum MenuOptions
    {
        SetUserConfigs,
        ClearUserConfigs,
        SendEmail,
        AutomaticEmail,
        Exit
    }

    internal enum AutomaticEmailActivator
    {
        ActivateAutomaticEmail,
        DeactivateAutomaticEmail
    }
}
