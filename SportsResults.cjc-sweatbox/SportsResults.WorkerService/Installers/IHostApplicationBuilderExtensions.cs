namespace SportsResults.WorkerService.Installers;

/// <summary>
/// Microsoft.Extensions.Hosting.IHostApplicationBuilder interface extension methods.
/// </summary>
public static class IHostApplicationBuilderExtensions
{
    /// <summary>
    /// Gets the installers for this application and performs the InstallServices method on each.
    /// </summary>
    /// <param name="builder">The IHostBuilder.</param>
    public static void InstallServicesInAssembly(this IHostApplicationBuilder builder)
    {
        var installers = typeof(Program).Assembly.ExportedTypes.
            Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).
            Select(Activator.CreateInstance).
            Cast<IInstaller>().
            ToList();

        installers.ForEach(installer => installer.InstallServices(builder));
    }
}
