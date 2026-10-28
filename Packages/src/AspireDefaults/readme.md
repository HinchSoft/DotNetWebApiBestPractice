# RDH.AspireDefaults

`RDH.AspireDefaults` provides initialization and migration worker base classes designed for hosted services and distributed systems, such as .NET Aspire applications. It manages background database migrations, data seeding, execution strategies, and OpenTelemetry activity tracing, terminating the host cleanly once initialization is complete.

## Installation

Install the package via the .NET CLI:

```bash
dotnet add package RDH.AspireDefaults
```

Or via the Package Manager Console:

```powershell
Install-Package RDH.AspireDefaults
```

---

## Key Features

- **Automated Migration Lifecycle:** `MigrationWorker<TContext>` runs database migrations inside a resilient EF Core execution strategy, applies seed data, and stops the application cleanly when finished.
- **OpenTelemetry Tracing:** Emits distributed tracing activities through an `ActivitySource` named `"Migrations"` to provide visibility in .NET Aspire dashboards and APM tools.
- **Transaction Safety:** Migration execution uses EF Core execution strategies to prevent partial or corrupted database states.

---

## Sample Code

### 1. Creating a Migration Worker

Inherit from `MigrationWorker<TContext>` to define migration and seeding logic:

```csharp
using DemoDomain.Models;
using DemoInfrastructure.Data;
using Hinchsoft.BestPractices.AspireDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace AspireMigration;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime)
    : MigrationWorker<ApplicationDbContext>(serviceProvider, hostApplicationLifetime)
{
    protected override Task MigrateDatabase(DatabaseFacade db, CancellationToken cancellationToken)
    {
        return db.MigrateAsync(cancellationToken);
    }

    protected override Task SeedDataAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var existingIds = dbContext.Items.Select(i => i.Id).ToHashSet();
        var newItems = _items.Where(i => !existingIds.Contains(i.Id));

        dbContext.Items.AddRange(newItems);
        dbContext.SaveChanges();

        return base.SeedDataAsync(dbContext, cancellationToken);
    }

    private readonly Item[] _items =
    [
        new Item(new ItemId(Guid.Parse("25B75482-20DE-47FF-9476-17302D9584B9")), "Beany cap", "Beany Cap / Hat size 1", 2, 7.99m)
    ];
}
```

### 2. Startup Registration in a Migration Console / Service

Register the worker as a hosted service in `Program.cs`:

```csharp
using AspireMigration;
using DemoInfrastructure.Data;
using Hinchsoft.BestPractices.AspireDefaults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Register the worker as a hosted service
builder.Services.AddHostedService<Worker>();

// Wire OpenTelemetry tracing for the migration activity source
builder.Services.AddOpenTelemetry()
    .WithTracing(tr => tr.AddSource(MigrationWorker<ApplicationDbContext>.ActivitySourceName));

// Configure database context
builder.AddNpgsqlDbContext<ApplicationDbContext>("demodb");

var host = builder.Build();
await host.RunAsync();
```
