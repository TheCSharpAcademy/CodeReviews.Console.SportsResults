namespace SportsResults.Service;

public interface IEmailSender
{
    public void SendEmail( string to, string subject, string email);
}