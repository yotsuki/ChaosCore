using ChaosCore.ModelBase;
using ChaosCore.RepositoryLib;
using Microsoft.EntityFrameworkCore;
//using MySQL.Data.EntityFrameworkCore.Extensions;

namespace demo.MysqlRepositoryLib
{
    public class DemoContext: ChaosBaseContext
    {
        public DemoContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbconfig = GetDbContextConfiguration();
            //optionsBuilder.UseMySQL(connStr);
            optionsBuilder.UseMySql(dbconfig.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().Property("ID").HasColumnType("char(36)");
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }

    }
}
