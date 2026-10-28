using Microsoft.EntityFrameworkCore;
using RDH.ApplicationLayer.Data;

namespace RDH.EFInfrastructure.Data;

public abstract class UnitOfWorkBase<TContext> : IUnitOfWork
    where TContext : DbContext
{
    private readonly TContext _dbContext;

    public UnitOfWorkBase(TContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}