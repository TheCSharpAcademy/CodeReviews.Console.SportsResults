using SportsResultNotifier.Models;

namespace SportsResultNotifier.Services;

public interface IMailerService
{
    bool SendEmail(Message message, string recipient);
}
