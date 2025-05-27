namespace SportResultsNotifier;

internal class Enums
{
    internal enum MainMenuOption
    {
        SetUser,
        SendResults,
        Exit,
    }

    internal enum UserMenuOption
    {
        SetUser,
        ResetUser,
        Return,
    }

    internal enum ResultMenuOption
    {
        LocalFolder,
        PaperCutServer,
        GmailSmtp,
        AllPossibleMethods,
        Return,
    }
}
