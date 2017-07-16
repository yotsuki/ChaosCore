using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.RepositoryLib
{
    public interface IUnitOfWork:IDisposable
    {
        long UserID { get; set; }
        void AddDbContext(IChaosCoreDbContext context);
        void SaveChanges();
        void SaveChanges(bool acceptAllChangesOnSuccess);
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
    }
}
