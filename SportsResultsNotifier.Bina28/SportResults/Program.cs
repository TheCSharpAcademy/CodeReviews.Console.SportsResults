using Hangfire.Server;

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

				// Add the Hangfire server that will run the jobs
				services.AddHangfireServer();

				// Add the Worker to execute tasks in the background
				services.AddHostedService<Worker>();
			});
}