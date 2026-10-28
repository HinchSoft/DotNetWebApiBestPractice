# RDH.EFInfrastructure

`RDH.EFInfrastructure` provides Entity Framework Core implementations of the repository and unit-of-work abstractions defined in `RDH.ApplicationLayer`. It bridges generic database operations and specific `DbContext` types without polluting domain or application layers.

## Installation

Install the package via the .NET CLI:

```bash
dotnet add package RDH.EFInfrastructure
```

Or via the Package Manager Console:

```powershell
Install-Package RDH.EFInfrastructure
```

---

## Key Features

- **Generic Repository Base:** `RepositoryBase<TEntity, TContext>` implements `IReadRepository<TEntity>` and `IWriteRepository<TEntity>` with asynchronous operations (`Add`, `AnyAsync`, `CountAsync`, `AsEnumerableAsync`, `AsyncEnumerable`, etc.).
- **Unit of Work Base:** `UnitOfWorkBase<TContext>` manages `DbContext.SaveChangesAsync` to guarantee single-commit transactions across multiple repository operations.
- **Clean Separation:** Domain entities and application handlers depend only on interfaces; EF Core specifics are encapsulated in this layer.

---

## Sample Code

### 1. Implementing a Concrete Repository

Derive from `RepositoryBase<TEntity, TContext>` bound to your application's `DbContext`:

```csharp
using RDH.EFInfrastructure.Data;

namespace DemoInfrastructure.Data;

internal class Repository<TEntity>(ApplicationDbContext context)
    : RepositoryBase<TEntity, ApplicationDbContext>(context)
    where TEntity : class
{
}
```

### 2. Implementing a Concrete Unit of Work

Derive from `UnitOfWorkBase<TContext>`:

```csharp
using RDH.EFInfrastructure.Data;

namespace DemoInfrastructure.Data;

internal class UnitOfWork(ApplicationDbContext context)
    : UnitOfWorkBase<ApplicationDbContext>(context)
{
}
```

### 3. Registering Repositories and Unit of Work in DI

Wire up the interfaces to your concrete implementations in your service configuration:

```csharp
using DemoInfrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using RDH.ApplicationLayer.Data;

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
```
