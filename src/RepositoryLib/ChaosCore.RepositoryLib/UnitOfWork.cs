using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;

namespace ChaosCore.RepositoryLib
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public long UserID { get; set; }
        private Dictionary<DatabaseFacade, IDbContextTransaction> _dicTransaction = null;
        private List<IChaosCoreDbContext> _lstDbContext = new List<IChaosCoreDbContext>();
        private IsolationLevel _isolationLevel = System.Data.IsolationLevel.ReadCommitted;
        /// <summary>
        /// 获取当前工作单元是否处于事务中
        /// </summary>
        public bool IsTransaction
        {
            get { return _dicTransaction!= null; }
        }

        /// <summary>
        /// 添加上下文
        /// </summary>
        /// <param name="context"></param>
        public void AddDbContext(IChaosCoreDbContext context)
        {
            if (!_lstDbContext.Contains(context))
            {
                _lstDbContext.Add(context);
                if (IsTransaction) {
                    var conn = context.GetDbConnection();
                    if (conn.State == ConnectionState.Closed) {
                        conn.Open();
                    }
                    if (context.Database.CurrentTransaction == null) {
                        _dicTransaction.Add(context.Database, context.Database.BeginTransaction());
                    } 
                }
            }
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            if (_dicTransaction != null)
            {
                //throw new ApplicationException("Cannot begin a new transaction while an existing transaction is still running. " +
                //                                "Please commit or rollback the existing transaction before starting a new one.");
                return;
            }
            OpenConnection();
            //_isolationLevel = isolationLevel;
            _dicTransaction = new Dictionary<DatabaseFacade, IDbContextTransaction>();
            foreach (var context in _lstDbContext)
            {
                if (context.Database.CurrentTransaction == null) {
                    _dicTransaction.Add(context.Database, context.Database.BeginTransaction());
                } 
            }
        }
        ///// <summary>
        ///// 开始事务
        ///// </summary>
        //public void BeginTransaction()
        //{
        //    BeginTransaction(IsolationLevel.ReadCommitted);
        //}
        /// <summary>
        /// 提交事务
        /// （事务失败将回滚)
        /// </summary>
        public void CommitTransaction()
        {
            if (_dicTransaction == null)
            {
                throw new Exception("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                foreach (var kvp in _dicTransaction)
                {
                    //kvp.Key.GetObjectContext()
                    kvp.Value.Commit();
                    //kvp.Key.AcceptAllChanges();
                }
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                ReleaseCurrentTransaction();
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackTransaction()
        {
            if (IsTransaction)
            {
                foreach (var kvp in _dicTransaction)
                {
                    kvp.Value.Rollback();
                    //kvp.Key.RefreshEntites();
                    //kvp.Key.GetObjectContext().AcceptAllChanges();
                }
                ReleaseCurrentTransaction();
            }
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        public void SaveChanges()
        {
            _lstDbContext.ForEach(c => {
                c.UserID = UserID;
                c.SaveChanges();
            });
        }
        /// <summary>
        /// 保存更改
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        public void SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _lstDbContext.ForEach(c => {
                c.UserID = UserID;
                c.SaveChanges(acceptAllChangesOnSuccess);
                });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            ReleaseCurrentTransaction();
            if (_lstDbContext != null) {
                foreach (var context in _lstDbContext) {
                    context.Dispose();
                }
                _lstDbContext = null;
            }
            
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 打开所有数据链接
        /// </summary>
        private void OpenConnection()
        {
            foreach (var context in _lstDbContext)
            {
                if (context.GetDbConnection().State != ConnectionState.Open)
                {
                    context.GetDbConnection().Open();
                }
            }
        }

        /// <summary>
        /// 释放事务
        /// </summary>
        private void ReleaseCurrentTransaction()
        {
            if (_dicTransaction != null)
            {
                foreach (var kvp in _dicTransaction)
                {
                    kvp.Value.Dispose();
                }
                _dicTransaction = null;
            }
        }

    }
}
