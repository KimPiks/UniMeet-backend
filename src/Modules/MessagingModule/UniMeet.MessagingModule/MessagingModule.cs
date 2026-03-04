using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem;
using Serilog;
using UniMeet.MessagingModule.Application;
using UniMeet.MessagingModule.Application.Messages;
using UniMeet.MessagingModule.Config;
using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.MessagingModule.Infrastructure;
using UniMeet.MessagingModule.Infrastructure.Conversations;
using UniMeet.MessagingModule.Infrastructure.Messages;
using UniMeet.Shared.Exceptions;
using UniMeet.Shared.Mediator.Extensions;

namespace UniMeet.MessagingModule;

public class MessagingModule : IModule
{
    public string Name { get; init; } = "MessagingModule";
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
        services.AddDbContext<MessagingContext>(options =>
        {
            options.UseNpgsql(_configuration.ConnectionString);
        });

        // Repositories
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();

        // Mediator handlers
        services.RegisterMediator(typeof(MessagingModuleApplication).Assembly);
    }

    private static Configuration ValidateConfiguration(IConfiguration configuration)
    {
        var connectionString = configuration["DbConnectionString"];

        if (string.IsNullOrEmpty(connectionString))
            throw new MissingConfigurationException("MessagingModule: DbConnectionString is not configured.");

        return new Configuration(connectionString);
    }
}
