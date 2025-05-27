using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using HtmlAgilityPack;
using Spectre.Console;
using SportResultsNotifier.Models;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SportResultsNotifier.Controllers;

internal class UserController
{
    internal static void SetUser(IUser user, Configuration config)
    {
        config.AppSettings.Settings["EmailAddress"].Value = user.Email;
        config.AppSettings.Settings["FirstName"].Value = user.FirstName;
        config.AppSettings.Settings["LastName"].Value = user.LastName;
        config.AppSettings.Settings["Type"].Value = user.Type;
        config.AppSettings.Settings["AppPassword"].Value = user.AppPassword;
        config.Save();
    }
}

internal class ResultsController
{
    internal static Results GetResults()
    {
        Results results = new();
        try
        {
    
    
            DateTime today = DateTime.Now;
    
            // test values
            // uncomment and hange the date if no results are avalaible
            // url = "https://www.basketball-reference.com/boxscores/?month=4&day=9&year=2025";
            //today = today.AddDays(-1);
    
            string url = $"https://www.basketball-reference.com/boxscores/?month={today.Month}&day={today.Day}&year={today.Year}";
            HtmlWeb web = new();
            HtmlDocument document = web.Load(url);
    
            Game previousGame = new();
            Game game = new();
            string outerHtml;
            int index = 1;
    
            List<HtmlNode> nodes = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[position()>0]").ToList();
            results.Body = "<hr><div><table  width=80% align=\"center\" style=\"table-layout:fixed\"><tbody><tr>\r\n    <td width=30% >Home</td><td width=30%>Away</td>\r\n</tr></tbody></table></div><hr>\r\n\r\n";
            foreach (HtmlNode node in nodes)
            {
                game.HomeTeam = node.SelectSingleNode($"//div[{index}]/table[1]/tbody/tr[1]/td[1]").InnerText;
                game.AwayTeam = node.SelectSingleNode($"//div[{index}]/table[1]/tbody/tr[2]/td[1]").InnerText;
                game.HomeScore = int.Parse(node.SelectSingleNode($"//div[{index}]/table[1]/tbody/tr[1]/td[2]").InnerText);
                game.AwayScore = int.Parse(node.SelectSingleNode($"//div[{index}]/table[1]/tbody/tr[2]/td[2]").InnerText);
                outerHtml = node.SelectSingleNode($"//*[@id=\"content\"]/div[3]/div[{index}]/table[1]/tbody/tr[1]/td[1]/a").OuterHtml;
                game.HomeTeamRef = outerHtml.Substring(16, 3);
                outerHtml = node.SelectSingleNode($"//*[@id=\"content\"]/div[3]/div[{index}]/table[1]/tbody/tr[2]/td[1]/a").OuterHtml;
                game.AwayTeamRef = outerHtml.Substring(16, 3);
    
                results = FormatResults(results, game, today, index);
                index++;
            }
            results.Subject = $"NBA Games result({today.Date.ToString("dd/MM/yyyy")})";
            return results;
        }
        catch { return results; }
    }
    
    private static Results FormatResults(Results results, Game game, DateTime today, int index)
    {
        string gameResume = "";
        string homeStyle = "";
        string awaysStyle = "";
        game.GameRef = $"https://www.basketball-reference.com/boxscores/{today.ToString("yyyyMMdd")}0{game.AwayTeamRef}.html";
        if (!game.NoWinner)
        {
            homeStyle = game.HomeWin ? "style=\"color: green;\"" : "style=\"color: red;\"";
            awaysStyle = game.HomeWin ? "style=\"color: red;\"" : "style=\"color: green;\"";
        }
    
        gameResume = "<hr><div><table  width=80% align=\"center\" style=\"table-layout:fixed\"><tbody><tr>\n" +
            $"\r\n\t<td WIDTH=10%><a href=\"https://www.basketball-reference.com/teams/{game.HomeTeamRef}/{today.Year}.html\" ><img height=30 class=\"teamlogo\" src=\"https://cdn.ssref.net/req/202504041/tlogo/bbr/{game.HomeTeamRef}-{today.Year}.png\" alt=\"Home team Logo\"/></a></td>" +
            $"\t<td width=30% {homeStyle}>{game.HomeTeam}</td>\r\t<td width=20%>{game.HomeScore}</td>\r" +
            $"\r\n\t<td WIDTH=10%><a href=\"https://www.basketball-reference.com/teams/{game.AwayTeamRef}/{today.Year}.html\" ><img height=30 class=\"teamlogo\" src=\"https://cdn.ssref.net/req/202504041/tlogo/bbr/{game.AwayTeamRef}-{today.Year}.png\" alt=\"Away team Logo\"/></a></td>" +
            $"\t<td width=30% {awaysStyle}>{game.AwayTeam}</td>\r\t<td width=20%>{game.AwayScore}</td>\r" +
            $"<td><a href=\"https://www.basketball-reference.com/boxscores/{today.ToString("yyyyMMdd")}0{game.AwayTeamRef}.html\">details</a></td>\r" +
            $"</tr></tbody></table></div>\n\r";
    
        results.Body += gameResume;
        return results;
    }
}

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
            SmtpSender sender = new(() => new SmtpClient(host: "localhost")
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
                .UsingTemplate(template.ToString(), user)
                .SendAsync();
    
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