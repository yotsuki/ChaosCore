using System;

namespace ChaosCore.RepositoryLib
{
    public class GenericRepositoryFactory : IGenericRepositoryFactory
    {
        public string ContextName { get; set; }
        public object CreateGenericRepository(Type type)
        {
            var repositoryType = typeof(Repository<>);
            var repositoryGenericType = repositoryType.MakeGenericType(type);
            var repository = (Repository)Activator.CreateInstance(repositoryGenericType); ;
            //repository.ContextName = ContextName;
            return repository;
        }
    }
}
