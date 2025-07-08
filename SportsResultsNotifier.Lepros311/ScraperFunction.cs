using HtmlAgilityPack;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;


public class ScraperFunction
{
    private readonly ILogger<ScraperFunction> _logger;

    public ScraperFunction(ILogger<ScraperFunction> logger)
    {
        _logger = logger;
    }

    [Function("ScraperFunction")]
    public async Task Run([TimerTrigger("%CRON_EXPRESSION%")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        try
        {

            // Scrape data from the website
            var url = Environment.GetEnvironmentVariable("SCRAPER_URL");
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Example XPath to select box scores
            var boxScores = htmlDoc.DocumentNode.SelectNodes("//table[contains(@class, 'stats_table')]");

            // Format the data
            var emailBody = new StringBuilder();

            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            CultureInfo userCulture = CultureInfo.CurrentCulture;
            string formattedDate = date.ToString(userCulture);

            if (boxScores != null)
            {
                emailBody.Append("<html><head><style>");
                emailBody.Append("table { border-collapse: collapse; width: 100%; max-width: 600px; margin: 0 auto; }");
                emailBody.Append("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
                emailBody.Append("tr:nth-child(even) { background-color: #f2f2f2; }");
                emailBody.Append("</style></head><body>");

                emailBody.Append("<h2 style='text-align: center; width: 100%;'>NBA Standings and Stats</h2>"); // Add heading

                emailBody.Append($"<h3 style='text-align: center; width: 100%;'>for {date}</h3>"); // Add heading

                foreach (var table in boxScores)
                {
                    // Remove text from caption tags
                    var captionNode = table.SelectSingleNode(".//caption");
                    if (captionNode != null)
                    {
                        captionNode.InnerHtml = ""; // Clear the caption text
                    }

                    // Extract relevant data from each score
                    var styledTable = table.OuterHtml
                        .Replace("<table", "<table style='border-collapse:collapse; width: 100%; max-width: 600px;'")
                        .Replace("<td", "<td style='border:1px solid #ddd;padding:8px' display:block; text-align:right;' data-label='Score'")
                        .Replace("<th", "<th style='background-color:#f2f2f2;border:1px solid #ddd;padding:8px'");

                    emailBody.Append(styledTable);
                }

                emailBody.Append("</body></html>");
            }

            // Send the email
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                string emailSenderAddress = Environment.GetEnvironmentVariable("EMAIL_SENDER_ADDRESS");
                string emailAppPassword = Environment.GetEnvironmentVariable("EMAIL_APP_PASSWORD");
                string emailRecipientPassword = Environment.GetEnvironmentVariable("EMAIL_RECIPIENT_ADDRESS");

                smtpClient.Credentials = new NetworkCredential(emailSenderAddress, emailAppPassword);

                smtpClient.EnableSsl = true;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSenderAddress),
                    Subject = "Daily NBA Standings",
                    Body = emailBody.ToString(),
                    IsBodyHtml = true // Critical for HTML formatting
                };

                mailMessage.To.Add(emailRecipientPassword);

                try
                {
                    smtpClient.Send(mailMessage);
                    _logger.LogInformation("\nEmail sent successfully!");
                }
                catch (SmtpException smtpEx)
                {
                    _logger.LogError($"SMTP error while sending email: {smtpEx.Message}");
                    _logger.LogError(smtpEx.StackTrace);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"\nFailed to send email: {ex.Message}");
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError($"HTTP request error: {httpEx.Message}");
            _logger.LogError(httpEx.StackTrace);
        }
        catch (HtmlWebException htmlEx)
        {
            _logger.LogError($"HTML parsing error: {htmlEx.Message}");
            _logger.LogError(htmlEx.StackTrace);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred: {ex.Message}");
            _logger.LogError(ex.StackTrace);
        }

    }

}