namespace SportsResultsNotifier.UI;

public class MainUI
{
    public static void WelcomeMessage()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Sport Results Notifier app!");
        Thread.Sleep(3000);
    }

    public static void ExitMessage()
    {
        Console.WriteLine("The app is closing. \n Thank you for using the Sport Results Notifier app!");
        Thread.Sleep(3000);
    }

    public static void LoadingMessage()
    {
        Console.Clear();
        Console.WriteLine("Loading ...");
    }

    public static void ErrorMessage()
    {
        throw new NotImplementedException();
    }

    public static void InformationMessage(string message)
    {
        Console.WriteLine(message);
    }
}