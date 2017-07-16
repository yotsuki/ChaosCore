namespace ChaosCore.RepositoryLib
{
    /// <summary>
    /// 大对象操作接口
    /// </summary>
    public interface ILargeObject
    {
        /// <summary>
        /// 取消大对象链接
        /// postgrepsql用
        /// </summary>
        /// <param name="fileoid">大对象ID</param>
        void LoUnlink(long fileoid);
    }
}
