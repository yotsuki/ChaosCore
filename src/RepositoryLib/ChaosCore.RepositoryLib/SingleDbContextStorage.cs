using System;
using System.Collections.Generic;
using System.Text;

namespace ChaosCore.RepositoryLib
{
    public class SingleDbContextStorage : IDbContextStorage
    {
        public IServiceProvider _serviceProvider = null;
        public SingleDbContextStorage() { }
        public SingleDbContextStorage(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public List<IChaosCoreDbContext> DbContextList { get; set; } = new List<IChaosCoreDbContext>();

        public event EventHandler DbContextAdded;

        public IChaosCoreDbContext GetByKey(string key)
        {
            return _serviceProvider.GetService(typeof(IChaosCoreDbContext)) as IChaosCoreDbContext;
        }

        public void SetByKey(string key, IChaosCoreDbContext context)
        {
            
        }
    }
}
