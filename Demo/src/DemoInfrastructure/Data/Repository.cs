using EFInfrastructure.Data;

namespace DemoInfrastructure.Data;

internal class Repository<TEntity>(ApplicationDbContext context)
    : RepositoryBase<TEntity, ApplicationDbContext>(context)
    where TEntity : class
{

}
