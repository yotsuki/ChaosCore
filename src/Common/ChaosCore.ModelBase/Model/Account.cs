using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase
{
    
    /// <summary>
    /// 账户实体
    /// </summary>
    public class Account : BaseGuidEntity, IBaseGuidEntity
    {
        /// <summary>
        /// 用户对象
        /// </summary>
        [Key]
        public virtual User User {get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 确认Token
        /// </summary>
        public string ConfirmationToken { get; set; }
        /// <summary>
        /// 已确认？
        /// </summary>
        public bool IsConfirmed { get; set; }
        /// <summary>
        /// 最后一次密码错误日期
        /// </summary>
        public DateTime LastPasswordFailureDate { get; set; } = DateTime.Now;

        public int PasswordFailuresSinceLastSuccess { get; set; }

        public DateTime PasswordChangedDate { get; set; } = DateTime.Now;

        public string PasswordSalt { get; set; }

        public string PasswordVerificationToken { get; set; }

        public DateTime PasswordVerificationTokenExpirationDate { get; set; } = DateTime.Now;

    }
}
