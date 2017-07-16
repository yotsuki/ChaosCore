using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase
{

    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IBaseEntity
    {

        /// <summary>
        /// 创建用户ID
        /// </summary>
        long Creator { get; set; }
        /// <summary>
        /// 最后更新用户ID
        /// </summary>
        long UpdateUserID { get; set; }
        /// <summary>
        /// 最后变更时间
        /// </summary>
        DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 数据创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
    public interface IBaseEntity<T>:IBaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        T ID { get; set; }
    }
    public interface IBaseLongEntity: IBaseEntity<long>
    {

    }
    public interface IBaseGuidEntity : IBaseEntity<Guid>
    {

    }
}
