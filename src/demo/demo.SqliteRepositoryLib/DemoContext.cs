using ChaosCore.ModelBase;
using ChaosCore.RepositoryLib;
using Labmem.EntityFrameworkCorePlus;
using Labmem.EntityFrameworkCorePlus.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace demo.SqliteRepositoryLib
{
    public class DemoContext : ChaosBaseContext, IPlusDbContext
    {
        public DemoContext()
        {
        }
        public IServiceProvider GetPlusServiceProvider()
        {
            var loader = new Labmem.EntityFrameworkCorePlus.Sqlite.SqliteServiceCollectionLoader();
            return loader.GetServiceCollection().BuildServiceProvider();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbconfig = GetDbContextConfiguration();
            optionsBuilder.UseSqlite(dbconfig.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var serviceProvider = GetPlusServiceProvider(); 
            var pb = modelBuilder.PlusBuild(serviceProvider);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Function> Functions { get; set; }

        public DbSet<SysConfig> SysConfigs { get; set; }
    }
}
