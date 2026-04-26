using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem;
using Serilog;
using UniMeet.MatchingModule.Application;
using UniMeet.MatchingModule.Config;
using UniMeet.MatchingModule.Domain.Likes;
using UniMeet.MatchingModule.Domain.Matches;
using UniMeet.MatchingModule.Infrastructure;
using UniMeet.MatchingModule.Infrastructure.Likes;
using UniMeet.MatchingModule.Infrastructure.Matches;
using UniMeet.Shared.Exceptions;
using UniMeet.Shared.Mediator.Extensions;

namespace UniMeet.MatchingModule;

public class MatchingModule : IModule
{
    public string Name { get; init; } = "MatchingModule";
    public bool Enabled { get; set; }
    private Configuration _configuration = null!;

    public void Start(IConfiguration configuration)
    {
        try
        {
            _configuration = ValidateConfiguration(configuration);
            Enabled = true;
        }
        catch (MissingConfigurationException e)
        {
            Log.Logger.Fatal("{Message}", e.Message);
            Enabled = false;
        }
    }

    public void RegisterServices(IServiceCollection services)
    {
        services.AddDbContext<MatchingContext>(options =>
        {
            options.UseNpgsql(_configuration.ConnectionString);
        });

        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();

        services.RegisterMediator(typeof(MatchingModuleApplication).Assembly);
    }

    private static Configuration ValidateConfiguration(IConfiguration configuration)
    {
        var connectionString = configuration["DbConnectionString"];

        if (string.IsNullOrEmpty(connectionString))
            throw new MissingConfigurationException("MatchingModule: DbConnectionString is not configured.");

        return new Configuration(connectionString);
    }
}
