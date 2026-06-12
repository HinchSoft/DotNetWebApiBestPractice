namespace ServiceManagement.Data;

public interface IWriteRepository<TEntity>:IReadRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);
}
