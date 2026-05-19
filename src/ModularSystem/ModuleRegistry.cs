using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ModularSystem;

/// <summary>
/// Discovers modules, starts enabled modules, and registers the modular-system entry points.
/// </summary>
public sealed class ModuleRegistry
{
    private static readonly object ResolverLock = new();
    private static bool _moduleDependencyResolverRegistered;

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
        EnsureModuleDependencyResolverRegistered();

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var assembliesToScan = AppDomain.CurrentDomain.GetAssemblies().ToList();

        LoadRuntimeAssembliesFromBaseDirectory(baseDirectory);

        var loadedByPath = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => !string.IsNullOrWhiteSpace(assembly.Location))
            .ToDictionary(assembly => assembly.Location, StringComparer.InvariantCultureIgnoreCase);

        var referencedPaths = Directory.GetFiles(baseDirectory, "*.dll");
        var toLoad = referencedPaths.Where(r =>
            Path.GetFileNameWithoutExtension(r).EndsWith("Module", StringComparison.InvariantCultureIgnoreCase)).ToList();

        foreach (var path in toLoad)
        {
            var assembly = loadedByPath.TryGetValue(path, out var loadedAssembly)
                ? loadedAssembly
                : AssemblyLoadContext.Default.LoadFromAssemblyPath(path);

            if (assembliesToScan.All(loaded => loaded.FullName != assembly.FullName))
            {
                assembliesToScan.Add(assembly);
            }
        }

        return assembliesToScan;
    }

    private static void LoadRuntimeAssembliesFromBaseDirectory(string baseDirectory)
    {
        var loadedAssemblyNames = AppDomain.CurrentDomain.GetAssemblies()
            .Select(assembly => assembly.GetName().Name)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        foreach (var path in Directory.GetFiles(baseDirectory, "*.dll"))
        {
            AssemblyName assemblyName;
            try
            {
                assemblyName = AssemblyName.GetAssemblyName(path);
            }
            catch (BadImageFormatException)
            {
                continue;
            }

            if (assemblyName.Name is null || loadedAssemblyNames.Contains(assemblyName.Name))
            {
                continue;
            }

            AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            loadedAssemblyNames.Add(assemblyName.Name);
        }
    }

    private static void EnsureModuleDependencyResolverRegistered()
    {
        lock (ResolverLock)
        {
            if (_moduleDependencyResolverRegistered)
            {
                return;
            }

            AssemblyLoadContext.Default.Resolving += ResolveModuleDependency;
            _moduleDependencyResolverRegistered = true;
        }
    }

    private static Assembly? ResolveModuleDependency(AssemblyLoadContext context, AssemblyName assemblyName)
    {
        var assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{assemblyName.Name}.dll");
        return File.Exists(assemblyPath)
            ? context.LoadFromAssemblyPath(assemblyPath)
            : null;
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
