using ChaosCore.CommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public static string[] Validation(this BaseEntity entity)
        {
            var ctx = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(entity, ctx, results);
            return results.Select(r => r.ErrorMessage).ToArray();
        }
    }
}
