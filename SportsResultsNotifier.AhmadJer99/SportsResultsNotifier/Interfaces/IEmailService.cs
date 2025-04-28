namespace SportsResultsNotifier.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body);
    public Task SendLocalEmailAsync(string to, string subject, string body);
}
