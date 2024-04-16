using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using WebScraper.Data;
using WebScraper.Interfaces;
using WebScraper.Repositories;
using WebScraper.Services;

namespace WebScraper;

internal class Program
{
    static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddDbContext<WebScraperContext>();
        builder.Services.AddScoped<IWebScraperService, BasketballScraperService>();
        builder.Services.AddScoped<DailyReportJob>();
        builder.Services.AddScoped<IBasketballGameRepository, BasketballGameRepository>();
        builder.Services.AddScoped<IEmailService, BasketballEmailService>();
        builder.Services.AddQuartz();
        builder.Services.AddQuartzHostedService(
            opt =>
            {
                opt.WaitForJobsToComplete = true;
            });

        IHost app = builder.Build();

        var _context = app.Services.GetRequiredService<WebScraperContext>();

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        var _job = JobBuilder.Create<DailyReportJob>()
            .WithIdentity("DailyReportJob", "JobGroup")
            .Build();

        var _trigger = TriggerBuilder.Create()
            .WithIdentity("RepeatingTrigger", "TriggerGroup")
            .WithSimpleSchedule(s => s.RepeatForever().WithIntervalInSeconds(120))
            .Build();

        var _schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
        var _scheduler = await _schedulerFactory.GetScheduler();
        await _scheduler.ScheduleJob(_job, _trigger);

        await app.RunAsync();

        await app.StopAsync();
    }
}
