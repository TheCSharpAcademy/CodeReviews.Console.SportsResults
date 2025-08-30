using SportsResultsNotifier;
using SportsResultsNotifier.Model;

public sealed class SportsResultsNotificationService
{
    public static async Task SendNotificationEmailAsync()
    {
        try
        {
            var results = await WebScraper.FetchSportsDataAsync();
            var body = $"NBA Results for {DateTime.Now.ToShortDateString()}\n\n";

            if(results != null)
            {
                foreach (var result in results)
                {
                    body += "----------\n" + result + "\n"
                        + "\n"
                        + result.TeamData[0].ToString() + "\n"
                        + result.TeamData[1].ToString() + "\n----------\n\n";
                }
            }
            else
            {
                body += "----------\n" +
                    "No games were played today." +
                    "\n----------\n\n";
            }

                body += "To stop this automated email, close the application on your desktop.";

            var email = new EmailData() { Body = body, Subject = $"Your Daily NBA Breakdown is here! - {DateTime.Now.ToShortDateString()}" };
            EmailManager.SendEmail(email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}



