using Sports_Results_Notifier.Models;

namespace Sports_Results_Notifier.Services;

public interface IEmailService
{
    public void SendEmail(Game game);
}
