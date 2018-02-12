using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ChaosCore.RepositoryLib
{
    public class DictionaryDbContextStorage: IDbContextStorage
    {
        public DictionaryDbContextStorage() { }
        public DictionaryDbContextStorage(IServiceProvider serviceProvider, IDictionary<string, Type> map )
        {
            _serviceProvider = serviceProvider;
            _typeMap = map;
        }
        private IServiceProvider _serviceProvider = null;
        private IDictionary<string, Type> _typeMap = null;
        public IDictionary<string, Type> TypeMap {
            get {
                if (_typeMap == null) {
                    _typeMap = new Dictionary<string, Type>();
                }
                return _typeMap;
            }
        }

        private IDictionary<string, IChaosCoreDbContext> _dicDbContexts = null;
        public IDictionary<string, IChaosCoreDbContext> DictDbContexts {
            get {
                if (_dicDbContexts == null) {
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
        public List<IChaosCoreDbContext> DbContextList {
            get {
                if (_dicDbContexts != null) {
                    return _dicDbContexts.Values.ToList();
                } else {
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
            if (DictDbContexts.ContainsKey(key)) {
                return DictDbContexts[key];
            } else {
                lock (_lock) {
                    if (!_dicDbContexts.ContainsKey(key)) {
                        if(_typeMap.TryGetValue(key,out Type type)) {
                            return _serviceProvider.GetService(type) as IChaosCoreDbContext;
                        } else {
                            return null;
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
            if (_dicDbContexts.ContainsKey(key)) {
                //_dicDbContexts[key] = context;
                throw new Exception("DbContext is Contains!");
            } else {
                lock (_lock) {
                    if (!_dicDbContexts.ContainsKey(key)) {
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
            if (!_typeMap.ContainsKey(name)) {
                _typeMap.Add(name, dbcontext.GetType());
            }
            if (_dbContextAdded != null) {
                _dbContextAdded.Invoke(dbcontext, EventArgs.Empty);
            }
        }

        private event EventHandler _dbContextAdded = null;
        /// <summary>
        /// 添加上下文事件
        /// </summary>
        public event EventHandler DbContextAdded {
            add {
                _dbContextAdded += value;
            }
            remove {
                _dbContextAdded -= value;
            }
        }
    }
}
