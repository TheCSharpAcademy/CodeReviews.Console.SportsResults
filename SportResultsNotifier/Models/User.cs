namespace SportResultsNotifier.Models;

internal interface IUser
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Type { get; set; }
    public string? AppPassword { get; set; }
}

internal class SimpleUser : IUser
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Type { get; set; }
    public string? AppPassword { get; set; }

    internal SimpleUser() { }
}

internal class GmailUser : IUser
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Type { get; set; }
    public string? AppPassword { get; set; }

    internal GmailUser() { }
}