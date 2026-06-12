using System;
using System.Collections.Generic;
using System.Linq;
using AspPresentation.Exceptions;
using AspPresentation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ServiceManagement.Services;
using System.Reflection;
using AspPresentation.LoggingRedactors;
using Domain.PIILogging;

// ReSharper disable once CheckNamespace
namespace Api.Common;

public static class ServiceExtensions
{
    /// <summary>
    /// Looks for and adds the endpoints defined in IEndpoint classes.
    /// </summary>
    /// <param name="assemblies">(Optional) The assemblies to scan for endpoints. If none are provided, the calling assembly is used.</param>
    public static TBuilder AddEndpoints<TBuilder>(this TBuilder builder, params Assembly[] assemblies)
        where TBuilder : IHostApplicationBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        if (assemblies.Length == 0)
        {
            assemblies = [Assembly.GetCallingAssembly()];
        }

        foreach (var assembly in assemblies)
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

    public static TBuilder AddApiResponses<TBuilder>(this TBuilder builder)
            where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IApiUtilities, ApiUtilities>();

        return builder;
    }

    public static TBuilder AddGlobalProblemDetails<TBuilder>(this TBuilder builder)
            where TBuilder : IHostApplicationBuilder
    {
        // Add Problem Details Service
        builder.Services.AddProblemDetails(opt =>
        {
            opt.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method}:{context.HttpContext.Request.Path}";
                context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
            };
        });

        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


        return builder;
    }

    public static IServiceCollection AddRedactionLogging(this IServiceCollection services)
    {
        services.AddRedaction(rb =>
        {
            rb.SetRedactor<FullStarRedactor>(PIIClassifications.Private);
            rb.SetRedactor<PartialStarRedactor>(PIIClassifications.Personal);
        });

        return services;
    }
}
