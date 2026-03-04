using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
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
using UniMeet.UserModule.Domain.Interests;
using UniMeet.UserModule.Domain.PasswordResetCodes;
using UniMeet.UserModule.Domain.RefreshTokens;
using UniMeet.UserModule.Domain.Services;
using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Domain.Users;
using UniMeet.UserModule.Infrastructure;
using UniMeet.UserModule.Infrastructure.ConfirmationCodes;
using UniMeet.UserModule.Infrastructure.Interests;
using UniMeet.UserModule.Infrastructure.PasswordResetCodes;
using UniMeet.UserModule.Infrastructure.RefreshTokens;
using UniMeet.UserModule.Infrastructure.UserDetails;
using UniMeet.UserModule.Infrastructure.Users;
using UniMeet.UserModule.Infrastructure.Services;

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
        services.AddScoped<IUserDetailRepository, UserDetailRepository>();
        services.AddScoped<IInterestRepository, InterestRepository>();
        services.AddScoped<IConfirmationCodeRepository, ConfirmationCodeRepository>();
        services.AddScoped<IPasswordResetCodeRepository, PasswordResetCodeRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IConfirmationLinkService, ConfirmationLinkService>(_ => new ConfirmationLinkService(_configuration.WebsiteUrl));
        services.AddSingleton<IPasswordResetLinkService, PasswordResetLinkService>(_ => new PasswordResetLinkService(_configuration.WebsiteUrl));
        services.AddSingleton<IJwtService, JwtService>(_ => new JwtService(_configuration.Auth.Secret, _configuration.Auth.Issuer, _configuration.Auth.Audience));
        services.AddSingleton<IProfilePictureValidator, ProfilePictureValidator>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        
        // Mediator
        services.RegisterMediator(typeof(UserModuleApplication).Assembly);
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration.Auth.Issuer,
                    ValidAudience = _configuration.Auth.Audience,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.Auth.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    // SignalR sends the token via query string instead of Authorization header
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            Error = "Unauthorized",
                            Message = "You are not authorized to access this resource.",
                            StatusCode = 401
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                };
            });
    }
    
    private static Configuration ValidateConfiguration(IConfiguration configuration)
    {
        var connectionString = configuration["DbConnectionString"];
        var websiteUrl = configuration["WebsiteUrl"];
        var secret = configuration["Auth:Secret"];
        var issuer = configuration["Auth:Issuer"];
        var audience = configuration["Auth:Audience"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new MissingConfigurationException("UserModule: DbConnectionString is not configured.");
        }

        if (string.IsNullOrEmpty(websiteUrl))
        {
            throw new MissingConfigurationException("UserModule: WebsiteUrl is not configured.");
        }

        if (string.IsNullOrEmpty(secret) ||
            string.IsNullOrEmpty(issuer) ||
            string.IsNullOrEmpty(audience))
        {
            throw new MissingConfigurationException("Auth configuration is not configured.");
        }

        var authConfiguration = new AuthConfiguration(secret, issuer, audience);
        return new Configuration(connectionString, websiteUrl, authConfiguration);
    }
}