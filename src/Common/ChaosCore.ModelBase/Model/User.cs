using ChaosCore.ModelBase;
using Labmem.EntityFrameworkCorePlus.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.ModelBase
{
    public class User: BaseLongEntity,IBaseLongEntity
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required]
        [StringLength(128,MinimumLength = 1)]
        public string Name { get; set; }
        /// <summary>
        /// 密码（MD5两次加密后)
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 别称
        /// </summary>
        [StringLength(32, MinimumLength = 1)]
        public string Alias { get; set; }
        [StringLength(32)]
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [RegularExpression(@"^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]+)+)$")]
        public string Email { get; set; }
        [StringLength(1024)]
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Face { get; set; }
        /// <summary>
        /// 用户性别
        /// 0 男
        /// 1 女
        /// </summary>
        [Range(0,1)]
        public int Sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool Lock { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        public string Memo { get; set; }

        public bool IsDeleted { get; set; }

        [StringLength(256)]
        public string WorkUnit { get; set; }

        [StringLength(18)]
        [RegularExpression(@"^\d{17}[0-9xX]$")]
        public string IDCard { get; set; }
        /// <summary>
        /// 一句话说明
        /// </summary>
        [StringLength(1024)]
        public string Description { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [StringLength(128)]
        public string Position { get; set; }
        /// <summary>
        /// 持有角色列表
        /// </summary>
        public virtual ICollection<UserRole> Roles { get; set; }

        //public virtual ICollection<ClientAuthorization> ClientAuthorizations { get; set; }
    }
}
