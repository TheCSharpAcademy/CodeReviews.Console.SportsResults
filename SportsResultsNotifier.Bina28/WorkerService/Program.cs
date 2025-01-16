using Hangfire;
using WorkerService;
using WorkerService.Services;
public class Program
{
	public static void Main(string[] args)
	{
		CreateHostBuilder(args).Build().Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureServices((hostContext, services) =>
			{
				// Add Hangfire and configure SQL Server storage
				services.AddHangfire(config =>
				{
					var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
					config.UseSqlServerStorage(connectionString);
				});

				// Add the Hangfire server
				services.AddHangfireServer();

				// Add the Worker
				services.AddHostedService<Worker>();

				// Register additional services
				services.AddSingleton<BasketballDataScraper>();
				services.AddSingleton<EmailService>();

			});
}
