namespace ChaosCore.RepositoryLib
{
    /// <summary>
    /// EntityFramework 配置操作
    /// </summary>
    public interface IORMConfig
    {
        /// <summary>
        /// 配置代理模式
        /// </summary>
        /// <param name="isCreationProxy">是否开启代理模式</param>
        void SetProxy(bool isCreationProxy);
        /// <summary>
        /// 配置延时加载
        /// </summary>
        /// <param name="isLoadingLazy">是否开启延时加载</param>
        void SetLazy(bool isLoadingLazy);
    }
}
