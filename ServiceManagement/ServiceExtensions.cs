using ServiceManagement.CQRS;
using System.Reflection;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    /// <summary>
    /// Registers CQRS (Command Query Responsibility Segregation) services and handlers with the dependency injection
    /// container.
    /// </summary>
    /// <remarks>This method scans the specified assembly for implementations of IQueryHandler,
    /// ICommandHandler, and ICommandHandler interfaces and registers them with the service collection. It also
    /// registers the ISender service for dispatching commands and queries.</remarks>
    /// <param name="services">The service collection to which CQRS services and handlers will be added.</param>
    /// <param name="assemblies">The assembly to scan for CQRS handler implementations. If not set, the calling assembly is used.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddCQRS(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies.Length==0)
        {
            assemblies = [Assembly.GetCallingAssembly()];
        }

        services.AddScoped<ISender, Sender>();

        var handlers = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>),
            typeof(IAsyncEnumerableQueryHandler<,>),
        };

        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly
                .GetTypes()
                .Where(type => !(type.IsAbstract || type.IsInterface))
                .SelectMany(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType && handlers.Contains(i.GetGenericTypeDefinition()))
                    .Select(i => new { Interface = i, Implementation = type }));

            foreach (var handler in handlerTypes)
            {
                services.AddScoped(handler.Interface, handler.Implementation);
            }
        }
        return services;
    }


}
