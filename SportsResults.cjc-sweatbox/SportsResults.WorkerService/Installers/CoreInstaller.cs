using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResults.Configurations;

namespace SportsResults.WorkerService.Installers;

/// <summary>
/// Register the services required by the core library to the DI container.
/// </summary>
public class CoreInstaller : IInstaller
{
    #region Methods

    public void InstallServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions<MailOptions>().Bind(builder.Configuration.GetSection(nameof(MailOptions)));
        builder.Services.AddOptions<ScraperServiceOptions>().Bind(builder.Configuration.GetSection(nameof(ScraperServiceOptions)));
        builder.Services.AddOptions<WorkerServiceOptions>().Bind(builder.Configuration.GetSection(nameof(WorkerServiceOptions)));
        }

    #endregion
}
