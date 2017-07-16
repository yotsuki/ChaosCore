namespace ChaosCore.RepositoryLib
{
    /// <summary>
    /// 实体操作类接口
    /// </summary>
    public interface IChaosRepository
    {
        /// <summary>
        /// 数据工厂
        /// </summary>
        IDbContextStorage DbStorage { get; set; }
    }
}
