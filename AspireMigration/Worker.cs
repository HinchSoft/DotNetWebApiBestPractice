using AspireDefaults;
using Demo.Infrastructure.Data;
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

}
