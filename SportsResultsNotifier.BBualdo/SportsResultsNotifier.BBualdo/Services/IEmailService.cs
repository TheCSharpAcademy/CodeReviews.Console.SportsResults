namespace SportsResultsNotifier.BBualdo.Services;

internal interface IEmailService
{
  Task SendEmailAsync(string email, string subject, string message);
}