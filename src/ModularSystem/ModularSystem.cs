using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularSystem;

public static class ModularSystem
{
    private static readonly ModuleRegistry Registry = new();

    public static void RunModules(IConfiguration configuration, IServiceCollection services)
        => Registry.RunModules(configuration, services);

    public static void SummarizeModules()
        => Registry.SummarizeModules();
}
