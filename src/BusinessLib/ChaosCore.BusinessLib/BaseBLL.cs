using System;
using System.Collections.Generic;
using System.Reflection;
using ChaosCore.ModelBase;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ChaosCore.CommonLib;
using ChaosCore.RepositoryLib;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using log4net;
using log4net.Config;
using System.IO;

namespace ChaosCore.BusinessLib
{
    /// <summary>
    /// BLL基类
    /// </summary>
    public abstract class BaseBLL : IBaseBLL, IDisposable, IBLLTransaction
    {
        private static IEnumerable<IStringLocalizer> s_CommonLocalizers = null;
        protected IStringLocalizer _localizer;
        public BaseBLL()
        {
            LogName = GetType().FullName;
        }
#if DEBUG
        public bool DebugMode { get; set; } = true;
#endif

        public BaseBLL(IServiceProvider provider):this()
        {
            if (provider.IsRoot()) {
                _serviceScope = provider.CreateScope();
                ServiceProvider = _serviceScope.ServiceProvider;
            }else {
                ServiceProvider = provider;
            }
             
            _localizer = provider.GetService(typeof(IStringLocalizer<>).MakeGenericType(GetType())) as IStringLocalizer;
            if(s_CommonLocalizers == null) {
                var commonResources = provider.GetServices<ICommonResource>();
                s_CommonLocalizers = commonResources
                    .Select(cr => (IStringLocalizer)provider.GetService(typeof(IStringLocalizer<>).MakeGenericType(cr.GetType())))
                    .ToArray();
            }
        }
        protected string Res(string name, params object[] parameters)
        {
            var str = (parameters == null || !parameters.Any()) ? _localizer[name] : _localizer[name, parameters];
            if (str.ResourceNotFound) {
                foreach(var localizer in s_CommonLocalizers.Reverse()) {
                    str = (parameters == null || !parameters.Any()) ? localizer[name] : localizer[name, parameters];
                    if (!str.ResourceNotFound) {
                        break;
                    }
                }
            }
            return str.Value;
        }
        /// <summary>
        /// 初始化日志
        /// </summary>
        private ILogger InitLogger()
        {
            var loggerFactory = ServiceProvider.GetService<ILoggerFactory>();
            return loggerFactory.CreateLogger(LogName);
        }
        public string LogName { get; set; }

        private ILogger _logger = null;
        /// <summary>
        /// 日志记录
        /// </summary>
        public ILogger Logger {
            get {
                return _logger ?? (_logger = InitLogger());
            }
            protected set {
                _logger = value;
            }
        }

        private IBusinessSession _session = null;
        /// <summary>
        /// 当前用户Session
        /// </summary>
        public IBusinessSession Session { get {
                return _session ?? (_session = ServiceProvider.GetService<IBusinessSession>());
            }
            set {
                _session = value;
            }
        }
        public long UserID {
            get {
                return Session.UserID;
            }
            set {
                Session.UserID = value;
            }
        }
        public string UserName {
            get {
                return Session.UserName;
            }
            set {
                Session.UserName = value;
            }
        }
        public IServiceProvider ServiceProvider { get; set; }
        private IServiceScope _serviceScope = null;

        public User CurrUser { get; set; }

        private IUnitOfWork _unitOfWork = null;
        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null) {
                    _unitOfWork = ServiceProvider.GetService<IUnitOfWork>();
                }
                return _unitOfWork;
            }
            set { _unitOfWork = value; }
        }

        private IRepositoryFactory _repositoryFactory = null;
        /// <summary>
        /// 实体操作对象工厂
        /// 用于自动创建IRepository对象
        /// </summary>
        public IRepositoryFactory RepositoryFactory
        {
            get
            {
                if (_repositoryFactory == null) {
                    _repositoryFactory = ServiceProvider.GetService(typeof(IRepositoryFactory)) as IRepositoryFactory; //new RepositoryFactory(ServiceProvider); 
                }
                return _repositoryFactory;
            }
            internal set {
                _repositoryFactory = value;
            }
        }

        /// <summary>
        /// 创建BLL
        /// BLL内部调用的其他BLL都建议使用此方法创建
        /// </summary>
        /// <typeparam name="T">BaseBLL的子类</typeparam>
        /// <returns></returns>
        protected T CreateBLL<T>()
            where T : BaseBLL
        {
            BaseBLL bll = Activator.CreateInstance<T>(); //Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName) as T;
            bll.ServiceProvider = this.ServiceProvider;
            return bll as T;
        }

        ///// <summary>
        ///// 从Spring配置中创建BLL
        ///// 主要用户调用其他系统的BLL
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //protected T CreateIBLL<T>()
        //    where T : IBaseBLL
        //{
        //    Type type = typeof(T);
        //    var typeinfo = type.GetTypeInfo();
        //    if (typeinfo.IsInterface) {
        //        var bll = ServiceProvider.GetService<T>();
        //        if (bll == null) {
        //            return default(T);
        //        }
        //        bll.UserID = this.UserID;
        //        BaseBLL baseBll = null;
        //        baseBll = bll as BaseBLL;
        //        if (baseBll != null) {
        //            baseBll.ServiceProvider = ServiceProvider;
        //        }
        //        return (T)bll;
        //    }
        //    return default(T);
        //}

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if(_serviceScope != null) {
                _serviceScope.Dispose();
            }
            if(_transaction != null) {
                _transaction.Dispose();
            }
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~BaseBLL()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) {
                if (this._unitOfWork != null) {
                    this._unitOfWork.Dispose();
                    this._unitOfWork = null;
                }
            }
        }

        /// <summary>
        /// 保存实体更改
        /// </summary>
        public virtual Result SaveChanges()
        {
            try {
                if (_unitOfWork != null) {
                    _unitOfWork.UserID = UserID;
                    _unitOfWork.SaveChanges();
                } else {
                    var list = ServiceProvider.GetService<IDbContextStorage>().DbContextList;
                    list.ForEach(dbcontext => {
                        dbcontext.UserID = UserID;
                        //dbcontext.OnLog = (log => Logger.Info(log));
                        dbcontext.SaveChanges();
                    });
                }
                return true;
            } catch (Exception ex) {
#if DEBUG
                if (DebugMode) {
                    throw ex;
                }
#endif 
                return Result.New(ErrorCode.OPTIMISTIC_CONCURRENCY, Res("SysRes.OptimisticConcurrency"));
            }
        }

        /// <summary>
        /// 保存实体更改
        /// </summary>
        /// <param name="saveOptions"></param>
        /// <returns></returns>
        public virtual Result SaveChanges(bool acceptAllChangesOnSuccess)
        {
            try {
                if (_unitOfWork != null) {
                    _unitOfWork.UserID = UserID;
                    _unitOfWork.SaveChanges(acceptAllChangesOnSuccess);
                } else {
                    var list = this._repositoryFactory.DbStorage.DbContextList;
                    list.ForEach(dbcontext => {
                        dbcontext.UserID = UserID;
                        dbcontext.SaveChanges(acceptAllChangesOnSuccess);
                    });
                }
                return true;
            } catch (DbUpdateConcurrencyException) {
                return Result.New(ErrorCode.OPTIMISTIC_CONCURRENCY, "DbUpdateConcurrencyException");
            }
        }
        private IDbContextTransaction _transaction = null;
        public void BeginTransaction()
        {
            UnitOfWork.BeginTransaction();
        }
        public void Commit()
        {
            UnitOfWork.CommitTransaction();
        }
        public void Rollback()
        {
            UnitOfWork.RollBackTransaction();
        }
        /// <summary>
        /// 保存实体更改
        /// </summary>
        /// <returns>保存结果true成功，false失败</returns>
        //public virtual bool DetectChangesBeforeSave()
        //{
        //    //SaveOptions saveOptions = SaveOptions.DetectChangesBeforeSave;

        //    return SaveChanges(saveOptions);
        //}
        /// <summary>
        /// 保存实体更改
        /// </summary>
        /// <returns>保存结果true成功，false失败</returns>
        public virtual bool NoneSaveChanges()
        {
            return SaveChanges(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>失败提交的数量</returns>
        public Result<int> AcceptAllChanges()
        {
            int count = 0;
            var list = this._repositoryFactory.DbStorage.DbContextList;
            var result = new Result<int>();
            list.ForEach(dbcontext => {
                try {
                    dbcontext.AcceptAllChanges();
                }
                catch (DbUpdateConcurrencyException) {
                    count++;
                }
            });
            result.Value = count;
            if (count > 0){
                result.Code = ErrorCode.OPTIMISTIC_CONCURRENCY;
                result.Message = "DbUpdateConcurrencyException";//SysRes.OptimisticConcurrency;
            }
            return result;
        }
        private Dictionary<Type, IChaosRepository> m_cacheRepository = new Dictionary<Type, IChaosRepository>();
        public IRepository<TEntity> GetRep<TEntity>()
            where TEntity :BaseEntity
        {
            //if (m_cacheRepository.ContainsKey(typeof(TEntity))) {
            //    return (IRepository<TEntity>)m_cacheRepository[typeof(TEntity)] ;
            //}
            //var rep = RepositoryFactory.CreateGenericRepository<TEntity>();
            //if (rep != null) {
            //    m_cacheRepository.Add(typeof(TEntity), rep);
            //}
            return ServiceProvider.GetService<IRepository<TEntity>>();
        }

        #region IBaseBLL 成员

        private string _errorMsg = string.Empty;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg
        {
            get
            {
                return _errorMsg;
            }
            set
            {
                _errorMsg = value;
            }
        }
        /// <summary>
        /// 错误编码
        /// </summary>
        public long ErrCode { get; set; }

        #endregion
    }
    /// <summary>
    /// BaseBLL的泛型实现
    /// </summary>
    /// <typeparam name="TEntity">实体类型BaseEntity</typeparam>
    public abstract class BaseBLL<TEntity> :BaseBLL
        where TEntity :BaseEntity
    {
        public BaseBLL() 
        {
        }
        public BaseBLL(IServiceProvider provider):base(provider)
        {

        }
        private IRepository<TEntity> _repository = null;
        /// <summary>
        /// 
        /// </summary>
        public virtual IRepository<TEntity> BllRepository
        {
            get{ return _repository??(_repository = ServiceProvider.GetService<IRepository<TEntity>>() );}
        }
#if DEBUG
        public virtual bool Clear()
        {
            BllRepository.Delete(a => true);
            return SaveChanges();
        }
        public virtual bool EntityUpdate(long id, Action<TEntity> entityUpFunc)
        {
            var entity = BllRepository.Find(id);
            if (entity == null) {
                return false;
            }
            if (entityUpFunc != null) {
                entityUpFunc.Invoke(entity);
                return SaveChanges();
            }
            return false;
        }
#endif

    }
}
