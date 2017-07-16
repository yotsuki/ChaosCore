using ChaosCore.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.RepositoryLib
{
    public interface IRepositoryFactory
    {
        IDbContextStorage DbStorage { get; set; }
        TRepository CreateRepository<TRepository>() where TRepository : IChaosRepository;
        IRepository<TEntity> CreateGenericRepository<TEntity>() where TEntity : BaseEntity;
    }
}
