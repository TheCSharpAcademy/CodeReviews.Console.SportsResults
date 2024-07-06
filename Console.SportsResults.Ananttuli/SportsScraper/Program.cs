using SportsScraper;

ConfigManager.Init();

Run();

static void Run()
{
    int MAX_RETRIES = 5;
    int remainingRetries = MAX_RETRIES;

    while (remainingRetries > 0)
    {
        try
        {
            Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");

            var summaries = Scraper.Scrape();
            var markup = EmailTemplate.Generate(summaries);

            Mailer.SendEmail(markup);

            File.WriteAllText($"email-sent-{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.html", markup);

            Console.Write($"Job completed at {DateTimeOffset.Now}");

            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Job failed at {DateTimeOffset.Now}. ERROR: {ex.Message}. Retrying ...");
        }
        finally
        {
            remainingRetries--;
        }
    }
}
