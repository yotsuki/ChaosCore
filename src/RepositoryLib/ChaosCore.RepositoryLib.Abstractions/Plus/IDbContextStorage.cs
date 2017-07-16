using System.Collections.Generic;
using System;

namespace ChaosCore.RepositoryLib
{
    /// <summary>
    /// DbContext数据仓库接口
    /// </summary>
    public interface IDbContextStorage
    {
        /// <summary>
        /// 根据KEY获取DbContext
        /// </summary>
        /// <param name="key">KEY</param>
        /// <returns>DbContext</returns>
        IChaosCoreDbContext GetByKey(string key);

        /// <summary>
        /// 根据KEY保存DbContext到仓库
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="context">DbContext</param>
        void SetByKey(string key, IChaosCoreDbContext context);

        /// <summary>
        /// 获取所有DbContext
        /// </summary>
        /// <returns>DbContext列表</returns>
        List<IChaosCoreDbContext> DbContextList { get;  }

        /// <summary>
        /// 上下文添加事件
        /// </summary>
        event EventHandler DbContextAdded;
    }
}