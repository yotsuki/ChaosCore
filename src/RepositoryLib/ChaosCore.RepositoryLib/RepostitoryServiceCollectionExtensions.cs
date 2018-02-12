using ChaosCore.RepositoryLib.Interceptors;
using ChaosCore.RepositoryLib.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                .AddScoped(typeof(IDbContextStorage), typeof(DbContextFactory))
                .AddSingleton<IDbContextInterceptor, UpdateDateTimeInterceptor>()
                .AddSingleton<IDbContextInterceptor, NewGuidInterceptor>();
        }

        public static IServiceCollection AddRepostitoryFromDbContext<TContext>(this IServiceCollection serviceCollection, IConfiguration config) where TContext: IChaosCoreDbContext
        {
            return serviceCollection
                .Configure<Dictionary<string, DbContextConfiguration>>(config.GetSection("DbContexts"))
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddScoped(typeof(IDbContextStorage), typeof(SingleDbContextStorage))
                .AddScoped(typeof(IChaosCoreDbContext), typeof(TContext))
                .AddSingleton<IDbContextInterceptor, UpdateDateTimeInterceptor>()
                .AddSingleton<IDbContextInterceptor, NewGuidInterceptor>();
        }
        public static IServiceCollection AddRepostitoryFromDictionary(this IServiceCollection serviceCollection, IDictionary<string, Type> dictionary, IConfiguration config = null) 
        {
            serviceCollection
                .Configure<Dictionary<string, DbContextConfiguration>>(config.GetSection("DbContexts"))
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddScoped<IDbContextStorage, DictionaryDbContextStorage>(p => new DictionaryDbContextStorage(p, dictionary))
                .AddSingleton<IDbContextInterceptor, UpdateDateTimeInterceptor>()
                .AddSingleton<IDbContextInterceptor, NewGuidInterceptor>();
            foreach(var type in dictionary.Values) {
                serviceCollection.AddScoped(type, type);
            }
            return serviceCollection;
        }
    }
}
