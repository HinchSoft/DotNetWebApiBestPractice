using Demo.Domain.Data;
using EFInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Data;

internal class Repository<TEntity>(ApplicationDbContext context)
    :RepositoryBase<TEntity,ApplicationDbContext>(context) where TEntity : class
{

}
