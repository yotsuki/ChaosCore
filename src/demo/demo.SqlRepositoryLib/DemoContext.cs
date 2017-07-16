using System;
using Microsoft.EntityFrameworkCore;
using ChaosCore.ModelBase;
using Labmem.EntityFrameworkCorePlus;
using Labmem.EntityFrameworkCorePlus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ChaosCore.RepositoryLib;

namespace demo.SqlRepositoryLib
{
    public class DemoContext: ChaosBaseContext,IPlusDbContext
    {
        public DemoContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbconfig = GetDbContextConfiguration();//DbContextConfigurations[GetType().Name].ConnectionString;
            //optionsBuilder.UseMySql(connStr);
            optionsBuilder.UseSqlServer(dbconfig.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.RoleId, ur.UserId });
            //modelBuilder.Entity<RoleFunction>().HasKey(rf => new { rf.RoleId, rf.FunctionId });
            //var type = modelBuilder.Entity<User>().Property("Eyes").Metadata.SqlServer().ColumnType;
            var serviceProvider = GetPlusServiceProvider();
            var pb = modelBuilder.Entity(typeof(User)).Property("Eyes").PlusBuild(serviceProvider);
            base.OnModelCreating(modelBuilder);
        }

        public IServiceProvider GetPlusServiceProvider()
        {
            var loader = new Labmem.EntityFrameworkCorePlus.SqlServer.SqlServerServiceCollectionLoader();
            return loader.GetServiceCollection().BuildServiceProvider();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Function> Functions { get; set; }
    }
}
