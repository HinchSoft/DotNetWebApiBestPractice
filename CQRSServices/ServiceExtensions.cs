using Api.Common.CQRS;
using CQRSServices.ApiResults;
using CQRSServices.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Api.Common;

public static class ServiceExtensions
{
    /// <summary>
    /// Looks for and adds the endpoints defined in IEndpoint classes.
    /// </summary>
    /// <param name="assemblys">(Optional) The assemblies to scan for endpoints. If none are provided, the calling assembly is used.</param>
    public static TBuilder AddEndpoints<TBuilder>(this TBuilder builder, params Assembly[] assemblys)
        where TBuilder : IHostApplicationBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        if (assemblys is null || !assemblys.Any())
        {
            assemblys = new Assembly[] { Assembly.GetCallingAssembly() };
        }

        foreach (var assembly in assemblys)
        {
            ServiceDescriptor[] serviceDescriptors = assembly
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                               type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray();

            builder.Services.TryAddEnumerable(serviceDescriptors);
        }
        return builder;
    }

    /// <summary>
    /// Registers CQRS (Command Query Responsibility Segregation) services and handlers with the dependency injection
    /// container.
    /// </summary>
    /// <remarks>This method scans the specified assembly for implementations of IQueryHandler<,>,
    /// ICommandHandler<>, and ICommandHandler<,> interfaces and registers them with the service collection. It also
    /// registers the ISender service for dispatching commands and queries.</remarks>
    /// <param name="services">The service collection to which CQRS services and handlers will be added.</param>
    /// <param name="assemblys">The assembly to scan for CQRS handler implementations. If not set, the calling assembly is used.</param>
    /// <returns>The same IServiceCollection instance so that additional calls can be chained.</returns>
    public static TBuilder AddCQRS<TBuilder>(this TBuilder builder, params Assembly[] assemblys)
            where TBuilder : IHostApplicationBuilder
    {
        if (assemblys is null || !assemblys.Any())
        {
            assemblys = [Assembly.GetCallingAssembly()];
        }

        builder.Services.AddScoped<ISender, Sender>();

        foreach (var assembly in assemblys)
        {
            var handlerTypes = assembly
                .GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface)
                .SelectMany(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition()==typeof(IRequestHandler<,>))
                    .Select(i => new { Interface = i, Implementation = type }));

            foreach (var handler in handlerTypes)
            {
                builder.Services.AddScoped(handler.Interface, handler.Implementation);
            }
        }
        return builder;
    }


    /// <summary>
    /// Maps the endpoints registered in the previous call to AddEndpoints.
    /// </summary>
    /// <param name="routeGroupBuilder">(Optional) The route group builder.</param>
    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        ArgumentNullException.ThrowIfNull(app);

        IEnumerable<IEndpoint> endpoints = app.Services
            .GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    public static TBuilder AddApiResponses<TBuilder>(this TBuilder builder, params Assembly[] assemblys)
            where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddScoped<IHttpResultService, HttpResultService>();

        return builder;
    }
}
