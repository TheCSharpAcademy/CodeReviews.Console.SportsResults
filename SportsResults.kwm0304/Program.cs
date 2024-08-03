using dotenv.net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SportsResults.kwm0304.Services;

namespace SportsResults.kwm0304;

public class Program
{
  public static void Main(string[] args)
  {
    DotEnv.Load();
    CreateHostBuilder(args).Build().Run();
  }

  public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureServices((hostContext, services) =>
            {
              services.AddQuartz(q =>
              {
                var jobKey = new JobKey("emailJob", "group1");
                q.AddJob<EmailService>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                      .ForJob(jobKey)
                      .WithIdentity("emailJob-trigger", "group1")
                      .WithCronSchedule("0 55 23 * * ?"));
              });
              services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
              services.AddSingleton<ScraperService>();
              services.AddSingleton<EmailService>();
            });
}