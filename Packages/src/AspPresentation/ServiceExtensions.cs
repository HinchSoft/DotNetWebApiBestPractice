using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using RDH.ApplicationLayer.Services;
using RDH.AspPresentation.Exceptions;
using RDH.AspPresentation.LoggingRedactors;
using RDH.AspPresentation.Services;
using RDH.Core.PIILogging;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    /// <summary>
    /// Looks for and adds the endpoints defined in IEndpoint classes.
    /// </summary>
    /// <param name="builder"></param>
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
    /// <param name="app"></param>
    /// <param name="routeGroupBuilder">(Optional) The route group builder.</param>
    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        ArgumentNullException.ThrowIfNull(app);

        IEnumerable<IEndpoint> endpoints = app.Services
            .GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    extension<TBuilder>(TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        public TBuilder AddApiResponses()
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IApiUtilities, ApiUtilities>();

            return builder;
        }

        public TBuilder AddGlobalProblemDetails()
        {
            // Add Problem Details Service
            builder.Services.AddProblemDetails(opt =>
            {
                opt.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Instance =
                        $"{context.HttpContext.Request.Method}:{context.HttpContext.Request.Path}";
                    context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);
                };
            });

            builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


            return builder;
        }
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