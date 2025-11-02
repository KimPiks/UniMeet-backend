using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem;

namespace UniMeet.UniversityModule;

public class UniversityModule : IModule
{
    public string Name { get; init; } = "UniversityModule";
    public bool Enabled { get; set; }
    
    public void Start(IConfiguration configuration)
    {
        this.Enabled = true;
    }

    public void RegisterServices(IServiceCollection services)
    {
        
    }
}