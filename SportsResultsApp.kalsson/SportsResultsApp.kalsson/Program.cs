using SportsResultsApp.kalsson.Services;

try
{
    Console.WriteLine("Scraping basketball game data...");
    var scraper = new DataScraper();
    var games = await scraper.ScrapeBasketballGamesAsync();

    if (games.Count == 0)
    {
        Console.WriteLine("No games found.");
        return;
    }

    Console.WriteLine($"Found {games.Count} game(s).");

    Console.WriteLine("Sending email...");
    var emailSender = new EmailSender();
    await emailSender.SendEmailAsync(games);
    Console.WriteLine("Email sent successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}