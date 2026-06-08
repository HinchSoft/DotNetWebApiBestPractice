using System;
using System.Collections.Generic;
using System.Text;
using ServiceManagement.Data;

namespace Demo.Domain.Data;

public interface IWriteRepository<TEntity>:IReadRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);
}
