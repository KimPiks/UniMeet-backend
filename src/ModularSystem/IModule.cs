using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularSystem;

public interface IModule
{
    string Name { get; init; }
    bool Enabled { get; set; }

    void Start(IConfiguration configuration);
    void RegisterServices(IServiceCollection services);
}