using System;
using System.Reflection;

namespace ChaosCore.RepositoryLib
{
    public class EfProviderConfigruation
    {
        public string Assembly { get; set; }

        public string DbContextOptionsExtension { get; set; }
        public string DbContextOptionsBuilder { get; set; }
    }
    public static class EfProviderConfigruationExtensions
    {
        public static ChaosCoreOption GetChaosCoreOption(this EfProviderConfigruation config)
        {
            //Type.GetType()
            var option = new ChaosCoreOption();
            var assembly = Assembly.Load(new AssemblyName(config.Assembly));
            option.ExtensionType = assembly.GetType(config.DbContextOptionsExtension);
            option.DbContextOptionsBuilderType = assembly.GetType(config.DbContextOptionsBuilder);
            return option;
        }
    }
}
