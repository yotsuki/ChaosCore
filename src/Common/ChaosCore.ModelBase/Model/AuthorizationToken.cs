using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChaosCore.ModelBase
{
    public class AuthorizationToken: BaseGuidEntity, IBaseGuidEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// Token字符串
        /// </summary>
        [StringLength(128)]
        public string Token { get; set; }
        /// <summary>
        /// RefreshToken字符串
        /// </summary>
        [StringLength(128)]
        public string RefreshToken { get; set; }
        [StringLength(16)]
        public string Key { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expires { get; set; } = DateTime.Now;

        [StringLength(64)]
        public string ClientIdentifier { get; set; }

        //public virtual ICollection<AuthorizationScope> Scopes { get; set; }
    }
}
