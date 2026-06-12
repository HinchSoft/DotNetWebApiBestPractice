using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServiceManagement.Data;

namespace EFInfrastructure.Data;

public abstract class RepositoryBase<TEntity, TContext> : IWriteRepository<TEntity>
    where TEntity : class where TContext : DbContext
{
    private readonly TContext _dbContext;

    protected RepositoryBase(TContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public void Add(TEntity entity)
    {
        _dbContext.Add(entity);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => _dbContext.Set<TEntity>().Where(predicate).AnyAsync(cancellationToken);

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        => _dbContext.Set<TEntity>().Where(predicate).CountAsync(cancellationToken);

    public Task<TEntity[]> AsEnumerableAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
        => _dbContext.Set<TEntity>().Where(predicate).ToArrayAsync(cancellationToken);

    public Task<TResult[]> AsEnumerableAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int, TResult>> selector,
        CancellationToken cancellationToken)
        => _dbContext.Set<TEntity>().Where(predicate)
            .Select(selector).ToArrayAsync(cancellationToken);

    public IAsyncEnumerable<TResult> AsyncEnumerable<TResult>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int, TResult>> selector)
        => _dbContext.Set<TEntity>().Where(predicate)
            .Select(selector).ToAsyncEnumerable();

    public async IAsyncEnumerable<System.Net.ServerSentEvents.SseItem<TResult>> AsyncEnumerableSse<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int, TResult>> selector)
    {
        var iq = _dbContext.Set<TEntity>().Where(predicate)
            .Select(selector);
        await foreach (var item in iq.AsAsyncEnumerable())
        {
            var sseItem = new System.Net.ServerSentEvents.SseItem<TResult>(item);
            yield return sseItem;
        }
    }
}
