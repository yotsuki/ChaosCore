using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChaosCore.ModelBase
{
    /// <summary>
    /// 用户组 实体
    /// </summary>
    public class UserGroup : BaseGuidEntity, IBaseGuidEntity
    {
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 角色列表
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }
        /// <summary>
        /// 持有用户
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
        /// <summary>
        /// 最后变更时间
        /// </summary>
        [ConcurrencyCheck]
        public override DateTime LastUpdateTime { get; set; }
    }
}
