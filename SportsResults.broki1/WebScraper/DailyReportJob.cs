using Quartz;
using WebScraper.Data;
using WebScraper.Interfaces;

namespace WebScraper;

public class DailyReportJob : IJob
{
    private readonly IWebScraperService _service;
    private readonly WebScraperContext _context;
    private readonly IEmailService _emailService;

    public DailyReportJob(IWebScraperService service, WebScraperContext context, IEmailService emailService)
    {
        this._service = service;
        this._context = context;
        this._emailService = emailService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            this._service.InsertGames();

            var games = this._context.BasketballGames.Where(game => game.Date == DateTime.Today.AddDays(-1)).ToList();

            var emailBody = this._emailService.CreateBody(games);

            var email = this._emailService.CreateEmail(emailBody);

            this._emailService.SendEmail(email);
        }

        catch (ArgumentNullException ex)
        {
            var email = this._emailService.CreateEmail("No games were played today.");

            this._emailService.SendEmail(email);
        }
    }
}
