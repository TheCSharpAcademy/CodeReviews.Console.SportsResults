using Spectre.Console;

namespace SportResultsNotifier;

internal class UserInputs
{
    internal static string ChooseModOptions()
    {
        return AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Choose an option below:")
            .AddChoices("Email", "First Name", "Last Name", "Type", "App Password"));
    }

    internal static string GetAppPassword()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter your app Password(Format: aaaa aaaa aaaa aaaa):")
            .Validate(x => x.Length == 19 && new List<char>() { x[4], x[9], x[14] }.All(y => y == ' ')));
    }

    internal static string GetEmailAddress()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("Enter an Email Address:")
            .Validate(x => x.IndexOf("@") == x.LastIndexOf("@") && x.Contains("@") && x.LastIndexOf("@") < x.LastIndexOf(".") &&
            x.Substring(x.LastIndexOf("@"), x.Length - x.LastIndexOf("@")).IndexOf(".") == x.Substring(x.LastIndexOf("@"), x.Length - x.LastIndexOf("@")).LastIndexOf(".") &&
            x.All(y => char.IsAsciiLetterLower(y) || char.IsAsciiDigit(y) || y == '.' || y == '@')));
    }

    internal static string GetName(string message)
    {
        return AnsiConsole.Prompt(new TextPrompt<string>(message)
            .Validate(x => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(x)));
    }
}