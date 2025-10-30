using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem;

namespace UniMeet.UserModule;

public class UserModule : IModule
{
    public string Name { get; init; } = "UserModule";
    public bool Enabled { get; set; }
    
    public void Start(IConfiguration configuration)
    {
        this.Enabled = true;
    }

    public void RegisterServices(IServiceCollection services)
    {
        
    }
}