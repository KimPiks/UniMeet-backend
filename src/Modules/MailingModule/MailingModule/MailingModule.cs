using MailingModule.Config;
using MailingModule.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem;
using Serilog;
using UniMeet.Shared.Exceptions;
using UniMeet.Shared.Mediator.Extensions;

namespace MailingModule;

public class MailingModule : IModule
{
    public string Name { get; init; } = "MailingModule";
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
        services.AddTransient<IEmailService, EmailService>(x => ActivatorUtilities.CreateInstance<EmailService>(x, _configuration));
        
        services.RegisterMediator(typeof(MailingModule).Assembly);
    }
    
    private static Configuration ValidateConfiguration(IConfiguration configuration)
    {
        var smtpConfiguration = configuration.GetSection("Smtp").Get<SmtpConfiguration>();

        if (smtpConfiguration == null)
        {
            throw new MissingConfigurationException("Smtp configuration is not configured.");
        }

        if (string.IsNullOrEmpty(smtpConfiguration.Host) ||
            string.IsNullOrEmpty(smtpConfiguration.Username) ||
            string.IsNullOrEmpty(smtpConfiguration.Password) ||
            string.IsNullOrEmpty(smtpConfiguration.SenderName) ||
            smtpConfiguration.Port == 0)
        {
            throw new MissingConfigurationException("Smtp configuration is not configured correctly.");
        }
        
        return new Configuration(smtpConfiguration);
    }
}