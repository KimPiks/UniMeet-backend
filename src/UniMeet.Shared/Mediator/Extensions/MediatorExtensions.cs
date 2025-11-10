using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using UniMeet.Shared.Abstractions;

namespace UniMeet.Shared.Mediator.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection RegisterMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<ServiceFactory>(sp => sp.GetRequiredService);

        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                                 i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                    .Select(i => new { Interface = i, Implementation = t }));

            foreach (var ht in handlerTypes)
            {
                services.AddScoped(ht.Interface, ht.Implementation);
            }
        }
        
        return services;
    }
}