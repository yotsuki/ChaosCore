using ChaosCore.Ioc;
using System;

namespace ChaosCore.RepositoryLib
{
    public class DbContextConfiguration
    {
        /// <summary>
        /// 提供器名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        public string Loader { get; set; }

        public object GetLoaderInstance()
        {
            if (string.IsNullOrWhiteSpace(Loader)) {
                return null;
            }
            var type = AssemblyExtension.GetType(Loader);
            return Activator.CreateInstance(type);
        }

        public string[] Entites { get; set; }
    }
}
