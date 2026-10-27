namespace RDH.ApplicationLayer.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}