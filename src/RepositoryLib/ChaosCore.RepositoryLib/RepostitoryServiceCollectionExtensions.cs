using ChaosCore.RepositoryLib.Interceptors;
using ChaosCore.RepositoryLib.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace ChaosCore.RepositoryLib
{
    public static class RepostitoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepostitory(this IServiceCollection serviceCollection, IConfiguration config)
        {
            return serviceCollection
                .Configure<Dictionary<string, DbContextConfiguration>>(config.GetSection("DbContexts"))
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddSingleton<IDbContextInterceptor, UpdateDateTimeInterceptor>()
                .AddSingleton<IDbContextInterceptor, NewGuidInterceptor>();
        }
    }
}
