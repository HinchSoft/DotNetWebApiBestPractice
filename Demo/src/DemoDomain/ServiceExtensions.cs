using System.Reflection;
using DemoDomain.Validators;
using FluentValidation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddValidatorsFromAssemblyContaining<UserValidator>();
        services.AddValidatorsFromAssemblies(assemblies);

        return services;
    }
}