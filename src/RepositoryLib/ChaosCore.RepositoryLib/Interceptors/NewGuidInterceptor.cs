using ChaosCore.ModelBase;
using ChaosCore.RepositoryLib.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.RepositoryLib.Interceptors
{
    public class NewGuidInterceptor : IDbContextInterceptor
    {
        public void OnSaveChanging(ChaosBaseContext dbcontext)
        {
            var entries = dbcontext.ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

            foreach (var a in entries) {
                var e = a.Entity as BaseEntity;
                if (e != null) {
                    if (a.State == EntityState.Added && e is IBaseGuidEntity && ((IBaseGuidEntity)e).ID == Guid.Empty) {
                        ((IBaseGuidEntity)e).ID = Guid.NewGuid();
                    }
                }
            }
        }
    }
}
