using ChaosCore.CommonLib;
using ChaosCore.ModelBase;
using ChaosCore.RepositoryLib.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ChaosCore.RepositoryLib.Interceptors
{
    public class UpdateDateTimeInterceptor : IDbContextInterceptor
    {
        public void OnSaveChanging(ChaosBaseContext dbcontext)
        {
            var entries = dbcontext.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var a in entries) {
                var e = a.Entity as BaseEntity;
                if (e != null) {
                    if (a.State == EntityState.Added && e is ICreateInfo) {
                        e.CreateTime = DateTimeHelper.GetNow();
                        if (dbcontext.UserID.HasValue) {
                            e.Creator = dbcontext.UserID.Value;
                        }
                        if (e is IUpdateInfo) {
                            e.UpdateDateTime();
                            e.UpdateUserID = e.Creator;
                        }
                    } else if (e is IUpdateInfo) {
                        if (DateTime.MinValue == e.CreateTime) {
                            e.CreateTime = DateTimeHelper.GetNow();
                        }
                        if (dbcontext.UserID.HasValue) {
                            e.UpdateUserID = dbcontext.UserID.Value;
                        }
                        e.UpdateDateTime();
                    }
                }
            }
        }
    }
}
