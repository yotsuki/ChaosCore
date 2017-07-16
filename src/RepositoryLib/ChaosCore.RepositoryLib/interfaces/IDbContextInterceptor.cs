using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.RepositoryLib.interfaces
{
    public interface IDbContextInterceptor
    {
        void OnSaveChanging(ChaosBaseContext dbcontext);
    }
}
