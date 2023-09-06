using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults;


var host = Host.CreateDefaultBuilder(args)
		.ConfigureServices((hostContext, services) =>
		{
			services.AddHostedService<SportResultsService>();
			services.AddSingleton<Scrape>();
			services.AddSingleton<Mail>();
		})
		.Build();
host.Run();
