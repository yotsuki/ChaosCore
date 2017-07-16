using ChaosCore.CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.ModelBase
{
    public static class BaseEntityExtensions
    {
        public static object GetID(this BaseEntity entity)
        {
            return null;
        }
        /// <summary>
        /// 更新最后修改时间
        /// </summary>
        public static void UpdateDateTime(this BaseEntity entity)
        {
            entity.LastUpdateTime = DateTimeHelper.GetNow();
        }
        public static void SetID(this BaseEntity entity, params object[] ids)
        {

        }
    }
}
