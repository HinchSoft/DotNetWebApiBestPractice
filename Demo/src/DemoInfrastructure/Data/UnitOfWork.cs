using RDH.EFInfrastructure.Data;

namespace DemoInfrastructure.Data;

internal class UnitOfWork(ApplicationDbContext context)
    : UnitOfWorkBase<ApplicationDbContext>(context)
{
}