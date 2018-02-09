namespace ChaosCore.RepositoryLib
{
    public interface IDbContextConfiguration
    {
        /// <summary>
        /// 提供器名称
        /// </summary>
        string TypeName { get; set; }
        /// <summary>
        /// 链接字符串
        /// </summary>
        string ConnectionString { get; set; }

        string Loader { get; set; }

        object GetLoaderInstance();

        string[] Entites { get; set; }
    }
}
