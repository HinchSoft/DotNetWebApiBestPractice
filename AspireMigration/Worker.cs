using AspireDefaults;
using DemoDomain.Models;
using DemoInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
        var addItems = _items.ExceptBy<Item, ItemId>(dbContext.Items.Select(i=>i.Id), i => i.Id);
        dbContext.Items.AddRange(addItems);

        dbContext.SaveChanges();
        return base.SeedDataAsync(dbContext, cancellationToken);
    }

    private Item[] _items = new[]
    {
        new Item(new ItemId(Guid.Parse("25B75482-20DE-47FF-9476-17302D9584B9")), "Beany cap", "Beany Cap / Hat size 1", 2, 7.99m)

    };
}
