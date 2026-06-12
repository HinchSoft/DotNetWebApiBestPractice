namespace ServiceManagement.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
