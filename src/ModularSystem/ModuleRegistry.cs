using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ModularSystem;

/// <summary>
/// Discovers modules, starts enabled modules, and registers the modular-system entry points.
/// </summary>
public sealed class ModuleRegistry
{
    private List<IModule> _modules = [];

    public void RunModules(IConfiguration configuration, IServiceCollection services)
    {
        services.AddScoped<IModuleRequestDispatcher, ModuleRequestDispatcher>();

        var assemblies = GetAssemblies();
        _modules = GetModules(assemblies);

        foreach (var module in _modules)
        {
            var moduleConfiguration = (IConfiguration)configuration.GetSection("Modules").GetSection(module.Name);
            if (string.IsNullOrEmpty(moduleConfiguration["Enabled"]))
            {
                Log.Logger.Error("Module configuration is not valid for: {ModuleName}", module.Name);
                continue;
            }

            if (bool.Parse(moduleConfiguration["Enabled"]!) == false)
            {
                continue;
            }

            Log.Logger.Information("Starting module: {ModuleName}", module.Name);
            module.Start(moduleConfiguration);

            if (!module.Enabled)
            {
                continue;
            }

            Log.Logger.Information("Registering services for module: {ModuleName}", module.Name);
            module.RegisterServices(services);
        }
    }

    public void SummarizeModules()
    {
        Log.Logger.Information("===== Modules Summary =====");
        foreach (var module in _modules)
        {
            Log.Logger.Information("Module: {ModuleName}, Enabled: {ModuleEnabled}", module.Name, module.Enabled);
        }

        Log.Logger.Information("===========================");
    }

    private static List<Assembly> GetAssemblies()
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

        var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
        var toLoad = referencedPaths.Where(r =>
            !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase) &&
            Path.GetFileName(r).Contains("Module", StringComparison.InvariantCultureIgnoreCase)).ToList();

        toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
        return loadedAssemblies;
    }

    private static List<IModule> GetModules(IList<Assembly> assemblies)
    {
        var modules = new List<IModule>();

        foreach (var assembly in assemblies)
        {
            try
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (!typeof(IModule).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract)
                    {
                        continue;
                    }

                    var module = (IModule)Activator.CreateInstance(type)!;
                    modules.Add(module);
                }
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (var exception in e.LoaderExceptions)
                {
                    Log.Logger.Fatal("Error loading assembly: {AssemblyName}, Exception: {Exception}",
                        assembly.FullName, exception);
                }
            }
        }

        return modules;
    }
}
