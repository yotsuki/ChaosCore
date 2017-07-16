using ChaosCore.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChaosCore.RepositoryLib
{
    /// <summary>
    /// 上下文工厂
    /// </summary>
    public class DbContextFactory : IDbContextStorage
    {
        public DbContextFactory(){}
        public DbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _ioc = _serviceProvider.GetService<IIocContext>();
        }
        private IServiceProvider _serviceProvider = null;
        private IIocContext _ioc = null;

        private IDictionary<string, IChaosCoreDbContext> _dicDbContexts = null;
        public IDictionary<string, IChaosCoreDbContext> DictDbContexts {
            get {
                if(_dicDbContexts == null) {
                    _dicDbContexts = new Dictionary<string, IChaosCoreDbContext>();
                }
                return _dicDbContexts;
            }
        }
        private object _lock = new object();

        /// <summary>
        /// 获取所有上下文
        /// </summary>
        /// <returns></returns>
        public List<IChaosCoreDbContext> DbContextList
        {
            get
            {
                if (_dicDbContexts != null)
                {
                    return _dicDbContexts.Values.ToList();
                }
                else
                {
                    return new List<IChaosCoreDbContext>();
                }
            }
        }
        
        /// <summary>
        /// 根据key获取对应上下文
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IChaosCoreDbContext GetByKey(string key)
        {
            if (DictDbContexts.ContainsKey(key)){
                return DictDbContexts[key];
            }else {
                lock (_lock) {
                    if (!_dicDbContexts.ContainsKey(key)) {
                        var context = _ioc.GetObject(key) as IChaosCoreDbContext;
                        if (context != null) {
                            context.ServiceProvider = _serviceProvider;
                            AddDbContext(key, context);
                            return context;
                        } else {
                            throw new Exception();
                        }
                    } else {
                        return _dicDbContexts[key];
                    }
                }
            }
        }
        /// <summary>
        /// 添加上下文
        /// </summary>
        /// <param name="key"></param>
        /// <param name="context"></param>
        public void SetByKey(string key, IChaosCoreDbContext context)
        {
            if (_dicDbContexts.ContainsKey(key))
            {
                //_dicDbContexts[key] = context;
                throw new Exception("DbContext is Contains!");
            }
            else
            {
                lock (_lock)
                {
                    if (!_dicDbContexts.ContainsKey(key))
                    {
                        AddDbContext(key, context);
                    }
                }
            }
        }

        private void AddDbContext(string name, IChaosCoreDbContext dbcontext)
        {
            var unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
            unitOfWork.AddDbContext(dbcontext);
            _dicDbContexts.Add(name, dbcontext);
            if (_dbContextAdded != null)
            {
                _dbContextAdded.Invoke(dbcontext, EventArgs.Empty);
            }
            
        }

        private event EventHandler _dbContextAdded = null;
        /// <summary>
        /// 添加上下文事件
        /// </summary>
        public event EventHandler DbContextAdded
        {
            add
            {
                _dbContextAdded += value;
            }
            remove
            {
                _dbContextAdded -= value;
            }
        }


    }
}
