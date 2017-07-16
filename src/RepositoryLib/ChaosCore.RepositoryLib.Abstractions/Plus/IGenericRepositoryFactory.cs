using System;

namespace ChaosCore.RepositoryLib
{
    /// <summary>
    /// 泛型实体操作对象工厂接口
    /// </summary>
    public interface IGenericRepositoryFactory
    {
        /// <summary>
        /// 创建泛型实体操作对象
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        object CreateGenericRepository(Type type);
    }
}
