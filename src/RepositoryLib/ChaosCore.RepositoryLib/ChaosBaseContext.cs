using ChaosCore.RepositoryLib.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ChaosCore.RepositoryLib
{

    public class ChaosBaseContext: DbContext, IChaosCoreDbContext
    {
        //private static readonly IConfigurationRoot ConnectionConfiguration = null;
        //protected static readonly IDictionary<string, DbContextConfiguration> DbContextConfigurations = null;
        static ChaosBaseContext()
        {
            //var file = new System.IO.FileInfo("appsettings.json");
            //var fullname = file.FullName;
            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile(fullname, optional: true, reloadOnChange: true);
            //ConnectionConfiguration = builder.Build();
            //DbContextConfigurations = ConnectionConfiguration.GetSection("DbContexts").Get<IDictionary<string, DbContextConfiguration>>();
            
        }
        private DbContextConfiguration _dbConfiguration = null;
        public DbContextConfiguration DbConfiguration { get {
                return _dbConfiguration ?? (_dbConfiguration = GetDbContextConfiguration());
            }
            set {
                _dbConfiguration = value;
            }
        }
        protected virtual DbContextConfiguration GetDbContextConfiguration()
        {
            var fullname = $"{GetType().FullName}, {GetType().GetTypeInfo().Assembly.GetName().Name}";
            if (ServiceProvider == null) {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: true, reloadOnChange: true)
                    .Build();
                ServiceProvider = new ServiceCollection().AddOptions().AddRepostitory(config).BuildServiceProvider();
            }
            var dbconfig = ServiceProvider.GetService<IOptions<Dictionary<string, DbContextConfiguration>>>().Value.Values.FirstOrDefault(c => c.TypeName == fullname);
            return dbconfig;
        }
        public ChaosBaseContext() { }
        public IServiceProvider ServiceProvider { get; set; }
        public string DefaultSchema { get; set; } = "dbo";
        public string SaveMessage { get; set; }
        public bool IsWriteLog { get; set; } = false;
        public long? UserID { get; set; }

        public Action<string> OnLog {get;set;}

        public override int SaveChanges()
            => SaveChanges(true);
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var interceptors = this.ServiceProvider.GetService<IEnumerable<IDbContextInterceptor>>();
            if (interceptors != null) {
                foreach (var interceptor in interceptors) {
                    interceptor.OnSaveChanging(this);
                }
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public IEnumerable<EntityEntry> GetChangeEntries()
        {
            return ChangeTracker.Entries();
        }

        public DbConnection GetDbConnection()
        {
            return  Database.GetDbConnection();
        }

        public void AcceptAllChanges()
        {
            ChangeTracker.AcceptAllChanges();
        }
        public void Unchanged<T>(T t) where T:class
        {
            Entry(t).State = EntityState.Unchanged;
        }
        
        //public object[] GetEntityKeys<T>(T entity)
        //{
        //    var entityType = Model.FindEntityType(typeof(T));
        //    var key = entityType.FindPrimaryKey();
        //}
    }
}
