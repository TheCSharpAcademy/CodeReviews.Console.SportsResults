using HtmlAgilityPack;

namespace WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private TimeSpan _timeSpan = TimeSpan.FromHours(24);

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                EmailService.GenerateEmailContent(GetGames());

                EmailService.SendEmail();

                List<GameModel> GetGames()
                {
                    var games = new List<GameModel>();
                    var webpage = new HtmlWeb().Load("https://www.basketball-reference.com/boxscores/");
                    var nodes = webpage.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[@class='game_summaries']/div/table[@class='teams']/tbody");

                    foreach (var node in nodes)
                    {
                        var game = new GameModel()
                        {
                            WinnerName = node.SelectSingleNode("tr[@class='winner']/td[1]").InnerText,
                            WinnerScore = Int32.Parse(node.SelectSingleNode("tr[@class='winner']/td[2]").InnerText),
                            LoserName = node.SelectSingleNode("tr[@class='loser']/td[1]").InnerText,
                            LoserScore = Int32.Parse(node.SelectSingleNode("tr[@class='loser']/td[2]").InnerText)
                        };
                        games.Add(game);
                    }
                    return games; //returns a list of games
                }
            }
            await Task.Delay(_timeSpan, stoppingToken);
        }
    }
}

