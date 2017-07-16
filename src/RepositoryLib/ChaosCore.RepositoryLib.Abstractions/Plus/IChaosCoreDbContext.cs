using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace ChaosCore.RepositoryLib
{
    /// <summary>
    /// 上下文扩展接口
    /// </summary>
    public interface IChaosCoreDbContext : IDisposable
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        long? UserID { get; set; }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="saveOptions"></param>
        /// <returns></returns>
        int SaveChanges(bool acceptAllChangesOnSuccess);
        ///// <summary>
        ///// 或者ObjectContext
        ///// </summary>
        ///// <returns></returns>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        //ObjectContext GetObjectContext();
        /// <summary>
        /// 日志
        /// </summary>
        Action<string> OnLog { get; set; }
        IServiceProvider ServiceProvider { get; set; }
        DatabaseFacade Database { get; }
        IEnumerable<EntityEntry> GetChangeEntries();
        DbConnection GetDbConnection();
        void AcceptAllChanges();
        void Unchanged<T>(T t) where T : class;
    }
}
