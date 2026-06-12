using System.Linq.Expressions;
using System.Net.ServerSentEvents;

namespace ServiceManagement.Data;

public interface IReadRepository<TEntity> where TEntity : class
{
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<TEntity[]> AsEnumerableAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken);

    Task<TResult[]> AsEnumerableAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int, TResult>> selector,
        CancellationToken cancellationToken);

    IAsyncEnumerable<TResult> AsyncEnumerable<TResult>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int, TResult>> selector);

    IAsyncEnumerable<SseItem<TResult>> AsyncEnumerableSse<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, int, TResult>> selector);
}
