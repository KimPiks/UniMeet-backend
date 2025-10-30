using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ModularSystem;

/// <summary>
/// Modular system is a system that allows you to create modules that can be enabled/disabled via configuration.
/// To add new module you need to create a new class that implements IModule interface.
/// Bootstrapper project should reference all modules and call RunModules method.
/// Configuration file should contain a section called "Modules" with a key for each module.
/// Each module configuration should contain a key "Enabled" with a value of true/false and additional configuration.
/// Example configuration you can find in ExampleConfiguration.json
/// </summary>
public static class ModularSystem
{
    private static List<IModule> _modules = [];

    public static void RunModules(IConfiguration configuration, IServiceCollection services)
    {
        var assemblies = GetAssemblies();
        _modules = GetModules(assemblies);

        foreach (var module in _modules)
        {
            // Get module configuration
            var moduleConfiguration = (IConfiguration)configuration.GetSection("Modules").GetSection(module.Name);
            if (string.IsNullOrEmpty(moduleConfiguration["Enabled"]))
            {
                Log.Logger.Error("Module configuration is not valid for: {ModuleName}", module.Name);
                continue;
            }

            // Check if module is enabled
            if (bool.Parse(moduleConfiguration["Enabled"]!) == false) continue;

            // Run a module
            Log.Logger.Information("Starting module: {ModuleName}", module.Name);
            module.Start(moduleConfiguration);

            // Register services
            if (!module.Enabled) continue;
            Log.Logger.Information("Registering services for module: {ModuleName}", module.Name);
            module.RegisterServices(services);
        }
    }

    public static void SummarizeModules()
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
            !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase) && r.Contains("Modules")).ToList();

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
                    if (!typeof(IModule).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract) continue;

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