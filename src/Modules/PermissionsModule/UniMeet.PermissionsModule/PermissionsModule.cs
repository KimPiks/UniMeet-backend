using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem;
using PermissionsModule.Application;
using PermissionsModule.Config;
using PermissionsModule.Domain.Groups;
using PermissionsModule.Domain.Permissions;
using PermissionsModule.Infrastructure;
using PermissionsModule.Infrastructure.Groups;
using PermissionsModule.Infrastructure.Permissions;
using Serilog;
using UniMeet.Shared.Exceptions;
using UniMeet.Shared.Mediator.Extensions;

namespace PermissionsModule;

public class PermissionsModule : IModule
{
    public string Name { get; init; } = "PermissionsModule";
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
        services.AddDbContext<PermissionsContext>(options =>
        {
            options.UseNpgsql(_configuration.ConnectionString);
        });
        
        // Repositories
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        
        // Mediator
        services.RegisterMediator(typeof(PermissionsModuleApplication).Assembly);

        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<PermissionsContext>();
            context.Database.Migrate();
        }
        
        this.RegisterDefaultGroups(services);
    }

    private void RegisterDefaultGroups(IServiceCollection services)
    {
        var groups = _configuration.DefaultGroups;
        
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<PermissionsContext>();
            if (context.Groups.Any() || context.Permissions.Any())
                return;
            
            foreach (var groupEntry in groups)
            {
                var groupConfig = groupEntry.Value;
                var groupName = groupConfig.Name;
                var permissions = groupConfig.Permissions;

                context.Groups.Add(new Group(groupName));
                context.SaveChanges();
                
                var group = context.Groups.First(g => g.Name == groupName);
                foreach (var permission in permissions)
                {
                    context.Permissions.Add(new Permission(group.Id, permission));
                }
                context.SaveChanges();
            }
        }
    }
    
    private static Configuration ValidateConfiguration(IConfiguration configuration)
    {
        var connectionString = configuration["DbConnectionString"];
        var defaultGroups = configuration.GetSection("DefaultGroups").Get<Dictionary<string, PermissionConfiguration>>();
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new MissingConfigurationException("PermissionsModule: DbConnectionString is not configured.");
        }

        if (defaultGroups == null)
        {
            throw new MissingConfigurationException("PermissionsModule: DefaultGroups is not configured.");
        }

        return new Configuration(connectionString, defaultGroups);
    }
}