using Demo.Domain.Data;
using Demo.Infrastructure.Data;
using ServiceManagement.CQRS;
using System.Reflection;
using ServiceManagement.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }


}
