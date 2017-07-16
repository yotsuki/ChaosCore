using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ChaosCore.ModelBase
{
    /// <summary>
    /// 实体基类
    /// </summary>
    [JsonObject()]
    [DataContract]
    public abstract class BaseEntity : IUpdateInfo, ICreateInfo
    {
        /// <summary>
        /// 构造
        /// </summary>
        public BaseEntity()
        {
        }
        

        [JsonIgnore]
        /// <summary>
        /// 创建者ID
        /// </summary>
        public virtual long Creator { get; set; }
        /// <summary>
        /// 最后更新用户ID
        /// </summary>
        public virtual long UpdateUserID { get; set; }

        /// <summary>
        /// 最后更新时间

        /// </summary>
        public virtual DateTime LastUpdateTime { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; } = DateTime.UtcNow;
 
    }

    [JsonObject()]
    public class BaseGuidEntity : BaseEntity
    {
        /// <summary>
        /// 构造
        /// </summary>
        public BaseGuidEntity()
        {
        }
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public virtual Guid ID { get; set; }
        [JsonIgnore]
        /// <summary>
        /// 创建者ID
        /// </summary>
        public override long Creator { get; set; }
        /// <summary>
        /// 最后更新用户ID
        /// </summary>
        public override long UpdateUserID { get; set; }

        /// <summary>
        /// 最后更新时间

        /// </summary>
        public override DateTime LastUpdateTime { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        /// <summary>
        /// 创建时间
        /// </summary>
        public override DateTime CreateTime { get; set; } = DateTime.UtcNow;

        //public override void SetID(params object[] ids)
        //{
        //    if(ids.Length >= 1 && ids[0] is Guid) {
        //        ID = (Guid)ids[0] ;
        //    }
        //}
    }

    [JsonObject()]
    public class BaseLongEntity : BaseEntity
    {
        /// <summary>
        /// 构造
        /// </summary>
        public BaseLongEntity()
        {
        }
        //public override object GetID()
        //{
        //    return ID;
        //}
        /// <summary>
        /// ID
        /// </summary>
        public virtual long ID { get; set; }
        [JsonIgnore]
        /// <summary>
        /// 创建者ID
        /// </summary>
        public override long Creator { get; set; }
        /// <summary>
        /// 最后更新用户ID
        /// </summary>
        public override long UpdateUserID { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public override DateTime LastUpdateTime { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        /// <summary>
        /// 创建时间
        /// </summary>
        public override DateTime CreateTime { get; set; } = DateTime.UtcNow;
        //public override void SetID(params object[] ids)
        //{
        //    if (ids.Length >= 1 && ids[0] is long) {
        //        ID = (long)ids[0];
        //    }
        //}
    }
}