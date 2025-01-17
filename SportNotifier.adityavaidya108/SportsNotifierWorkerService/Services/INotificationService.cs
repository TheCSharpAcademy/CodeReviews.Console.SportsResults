using HtmlAgilityPack;
using SportsNotifierWorkerService.Models;

namespace SportsNotifierWorkerService;

public interface INotificationService
{
    void StartApp();
    EmailDataFormat GetData();
    void SendEmail(string toEmail, string? subject, string? body);

    string GenerateEmailBody(EmailDataFormat data);
    string GetInnerTextOrDefault(HtmlNodeCollection nodes, string defaultValue = "");
}