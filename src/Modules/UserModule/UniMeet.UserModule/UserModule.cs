using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem;
using Serilog;
using UniMeet.Shared.Exceptions;
using UniMeet.Shared.Mediator.Extensions;
using UniMeet.UserModule.Application;
using UniMeet.UserModule.Application.Services;
using UniMeet.UserModule.Config;
using UniMeet.UserModule.Domain.ConfirmationCodes;
using UniMeet.UserModule.Domain.Services;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Infrastructure;
using UniMeet.UserModule.Infrastructure.ConfirmationCodes;
using UniMeet.UserModule.Infrastructure.Users;

namespace UniMeet.UserModule;

public class UserModule : IModule
{
    public string Name { get; init; } = "UserModule";
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
        services.AddDbContext<UserContext>(options =>
        {
            options.UseNpgsql(_configuration.ConnectionString);
        });
        
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IConfirmationCodeRepository, ConfirmationCodeRepository>();
        
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IConfirmationLinkService, ConfirmationLinkService>(_ => new ConfirmationLinkService(_configuration.WebsiteUrl));

        // Mediator
        services.RegisterMediator(typeof(UserModuleApplication).Assembly);
    }
    
    private static Configuration ValidateConfiguration(IConfiguration configuration)
    {
        var connectionString = configuration["DbConnectionString"];
        var websiteUrl = configuration["WebsiteUrl"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new MissingConfigurationException("UserModule: DbConnectionString is not configured.");
        }

        if (string.IsNullOrEmpty(websiteUrl))
        {
            throw new MissingConfigurationException("UserModule: WebsiteUrl is not configured.");
        }

        return new Configuration(connectionString, websiteUrl);
    }
}