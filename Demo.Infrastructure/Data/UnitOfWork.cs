using Demo.Domain.Data;
using EFInfrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Infrastructure.Data;

internal class UnitOfWork(ApplicationDbContext context)
    :UnitOfWorkBase<ApplicationDbContext>(context)
{

}
