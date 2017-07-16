using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase
{
    public class Function : BaseLongEntity, IBaseLongEntity
    {
        [StringLength(64)]
        public string FuncName { get; set; }
        [StringLength(64)]  
        public string FuncCode { get; set; }

        public long ParentFuncID { get; set; }
        //public virtual Function ParentFunction { get; set; }
        public bool IsMenu { get; set; }
        public int OrderNo { get; set; }

        [StringLength(256)]
        public string Url { get; set; }
        [StringLength(64)]
        public string Icon { get; set; }
        /// <summary>
        /// 所属角色列表
        /// </summary>
        public virtual ICollection<RoleFunction> Roles { get; set; }
    }
}
