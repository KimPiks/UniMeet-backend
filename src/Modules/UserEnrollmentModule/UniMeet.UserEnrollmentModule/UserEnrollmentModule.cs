﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem;
using Serilog;
using UniMeet.Shared.Exceptions;
using UniMeet.Shared.Mediator.Extensions;
using UniMeet.UserEnrollmentModule.Application;
using UniMeet.UserEnrollmentModule.Config;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;
using UniMeet.UserEnrollmentModule.Infrastructure;
using UniMeet.UserEnrollmentModule.Infrastructure.UserAffiliation;

namespace UniMeet.UserEnrollmentModule;

public class UserEnrollmentModule : IModule
{
    public string Name { get; init; } = "UserEnrollmentModule";
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
        services.AddDbContext<UserEnrollmentContext>(options =>
        {
            options.UseNpgsql(_configuration.ConnectionString);
        });
        
        // Repositories
        services.AddScoped<IUserAffiliationRepository, UserAffiliationRepository>();
        
        services.RegisterMediator(typeof(UserEnrollmentModuleApplication).Assembly);
    }
    
    private static Configuration ValidateConfiguration(IConfiguration configuration)
    {
        var connectionString = configuration["DbConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new MissingConfigurationException("UserModule: DbConnectionString is not configured.");
        }

        return new Configuration(connectionString);
    }
}