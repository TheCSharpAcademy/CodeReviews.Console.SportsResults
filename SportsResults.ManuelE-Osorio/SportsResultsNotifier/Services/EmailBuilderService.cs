using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using SportsResultsNotifier.Models;
using SportsResultsNotifier.Validation;

namespace SportsResultsNotifier.Services;

public class EmailBuilderService
{
    private readonly MailboxAddress SourceEmail;
    private readonly string? SourceEmailPassword;
    private readonly string? Host;
    private readonly int Port;
    private readonly MailboxAddress? DestinationEmail;

    public EmailBuilderService(AppVars appVars)
    {
        if(DataValidation.EmailValidation(appVars.SourceEmail, out _))
            SourceEmail = new("SourceEmail", appVars.SourceEmail);
        else
            throw new Exception($"The email {appVars.SourceEmail} from appsettings.json is invalid.");

        SourceEmailPassword = appVars.SourceEmailPassword;
        Host = appVars.Host;
        Port = appVars.Port;

        if(DataValidation.EmailValidation(appVars.DestinationEmail, out _))
            DestinationEmail = new("DestinationEmail", appVars.DestinationEmail);
        else
            throw new Exception($"The email {appVars.DestinationEmail} from appsettings.json is invalid.");
    }

    public async Task<bool> SendEmail(DateOnly date, List<SportResults> sportResults)
    {
        var message = BuildEmailBody(date, sportResults);

        using var client = new SmtpClient();
        client.Connect (Host, Port, SecureSocketOptions.Auto);
        client.Authenticate(SourceEmail.Address, SourceEmailPassword);
        var response = await client.SendAsync (message);
        client.Disconnect (true);
        if (response.Contains("OK", StringComparison.InvariantCultureIgnoreCase))
            return true;
        else 
            return false;
    }

    private MimeMessage BuildEmailBody(DateOnly date, List<SportResults> sportResults)
    {    
        var message = new MimeMessage ();
        message.From.Add (SourceEmail);
        message.To.Add (DestinationEmail);
        message.Subject = $"BasketBall Results for {date:yyyy/MM/dd}";

        string header = @"<style type=""text/css"">
            .tg  {border-collapse:collapse;border-spacing:0;}
            .tg td{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px;
            overflow:hidden;padding:10px 5px;word-break:normal;}
            .tg th{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px;
            font-weight:normal;overflow:hidden;padding:10px 5px;word-break:normal;}
            .tg .tg-18eh{border-color:#000000;font-weight:bold;text-align:center;vertical-align:middle}
            .tg .tg-mnhx{background-color:#fe0000;text-align:left;vertical-align:top}
            .tg .tg-riiz{background-color:#34ff34;border-color:#000000;text-align:left;vertical-align:top}
            .tg .tg-73oq{border-color:#000000;text-align:left;vertical-align:top}
            </style>
            <table class=""tg"">
            <tbody>";
        
        string body = HtmlBuilder(sportResults);

        string footer = @"</tbody>
            </table>";

        message.Body = new TextPart ("html") {
            Text = header + body + footer,
        };

        return message;
    }

    private static string HtmlBuilder(List<SportResults> sportResults)
    {
        string messageBody = "";
        for(int i = 0; i < sportResults.Count; i++)
        {
            messageBody +=  $"<tr>"+
                $"<td class=\"tg-18eh\" rowspan=\"2\">Game No. {i+1}</td>"+
                $"<td class=\"tg-riiz\"><span style=\"font-weight:bold;color:#333\">Winner : {sportResults[i].WinnerTeamName}"+
                $" {sportResults[i].WinnerTeamScore}</span></td>"+
                "</tr>"+
                "<tr>"+
                $"<td class=\"tg-mnhx\"><span style=\"font-weight:bold;color:#333\">Loser : {sportResults[i].LoserTeamName}"+
                $" {sportResults[i].LoserTeamScore}</span></td>"+
                "</tr>";
        }
        return messageBody;
    }
}
