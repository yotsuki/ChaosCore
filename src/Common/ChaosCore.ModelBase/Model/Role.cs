using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChaosCore.ModelBase
{
    public enum RoleType
    {
        Nomarl = 0,
        Administrator = 1,
        Customer = 2
    }
    /// <summary>
    /// 角色实体
    /// </summary>
    public class Role : BaseLongEntity
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [StringLength(32)]
        public string RoleName { get; set; }
        /// <summary>
        /// 持有用户列表
        /// </summary>
        public virtual ICollection<UserRole> Users { get; set; }

        public virtual ICollection<RoleFunction> Functions { get; set; }
        public RoleType Type { get; set; }

    }
}
