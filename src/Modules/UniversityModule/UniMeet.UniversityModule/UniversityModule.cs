using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem;
using Serilog;
using UniMeet.Shared.Exceptions;
using UniMeet.UniversityModule.Config;
using UniMeet.UniversityModule.Infrastructure;

namespace UniMeet.UniversityModule;

/// <summary>
/// The university module provides a list of universities, along with their departments and fields of study.
/// It also stores information about allowed email domains for registration.
/// </summary>
public class UniversityModule : IModule
{
    public string Name { get; init; } = "UniversityModule";
    public bool Enabled { get; set; }
    private Configuration _configuration = null!;
    
    public void Start(IConfiguration configuration)
    {
        try
        {
            _configuration = ValidateConfiguration(configuration);
            this.Enabled = true;
        }
        catch (MissingConfigurationException e)
        {
            Log.Logger.Fatal("{Message}", e.Message);
            this.Enabled = false;
        }
    }

    public void RegisterServices(IServiceCollection services)
    {
        services.AddDbContext<UniversityContext>(options =>
        {
            options.UseNpgsql(_configuration.ConnectionString);
        });
    }
    
    private static Configuration ValidateConfiguration(IConfiguration configuration)
    {
        var connectionString = configuration["DbConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new MissingConfigurationException("UniversityModule: DbConnectionString is not configured.");
        }

        return new Configuration(connectionString);
    }
}