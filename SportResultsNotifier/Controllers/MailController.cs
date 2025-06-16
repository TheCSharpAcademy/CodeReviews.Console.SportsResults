using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using Spectre.Console;
using SportResultsNotifier.Models;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace SportResultsNotifier.Controllers;

internal class MailController
{
    internal static async Task SendGmailSmtpAsync(Results results, IUser user)
    {
        using MailMessage mail = new();
        {
            mail.From = new MailAddress(user.Email);
            mail.To.Add(user.Email);
            mail.Subject = results.Subject;
            mail.Body = results.Body;
            mail.IsBodyHtml = true;
        }
        try
        {
            using SmtpClient client = new("smtp.gmail.com", 587);
            {
                client.Credentials = new NetworkCredential(user.Email, user.AppPassword);
                client.EnableSsl = true;
                await client.SendMailAsync(mail);
                AnsiConsole.MarkupLine("[Blue]Gmail[/] mail sent successfully.");
            }
        }
        catch (Exception ex) { AnsiConsole.MarkupLine($"An [Red]error[/] occured with the [red]gmail[/] mail:\n\n{ex.Message}"); }
    }

    internal static async Task SendLocalFolderAsync(Results results, IUser user)
    {
        try
        {
            string pickupDirectoryLocation = $"{AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.Length - 18)}\\PickupDirectory";
            SmtpSender sender = new(() => new SmtpClient(host: "localhost")
            {
                EnableSsl = false,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = pickupDirectoryLocation,
                // can be set to 25 for testing purpose
                Port = 587,
            });
            Email.DefaultSender = sender;

            StringBuilder template = new();
            template.AppendLine(results.Body);

            SendResponse response = await Email.From(user.Email, $"{user.FirstName} {user.LastName}")
                .To(user.Email, $"{user.FirstName} {user.LastName}")
                .Subject(results.Subject)
                .UsingTemplate(template.ToString(), user)
                .SendAsync();
            AnsiConsole.MarkupLine(response.Successful ? "[blue]Local[/] mail sent successfully" : $"[red]An error[/] occuer with the [red]local[/] mail:\n\n{response.ErrorMessages.First()}");
        }
        catch (Exception ex) { AnsiConsole.WriteLine(ex.Message); }
    }

    internal static async Task SendPapercutAsync(Results results, IUser user)
    {
        try
        {
            SmtpSender sender = new(() => new SmtpClient("localhost")
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 25,
            });
            Email.DefaultSender = sender;

            StringBuilder template = new();
            template.AppendLine(results.Body);

            SendResponse response = await Email.From(user.Email, $"{user.FirstName} {user.LastName}")
                 .To(user.Email, $"{user.FirstName} {user.LastName}")
                 .Subject(results.Subject)
                 .UsingTemplate(template.ToString(), user).SendAsync();

            AnsiConsole.MarkupLine(response.Successful ? "[blue]Papercut[/] mail sent successfully." : $"[red]An error[/] occured with the [red]papercut[/] mail:\n\n{response.ErrorMessages.First()}");
        }
        catch (Exception ex) { AnsiConsole.WriteLine(ex.Message); }
    }

    internal static async Task SendWithAllMethodsAsync(Results results, IUser user)
    {
        await SendPapercutAsync(results, user);
        await SendLocalFolderAsync(results, user);
        if (user.Type == "Gmail") await SendGmailSmtpAsync(results, user);
    }
}
