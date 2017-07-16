using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.ModelBase
{
    public class UserRole
    {
        [Key]
        public long UserID { get; set; }
        [Key]
        public long RoleID { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
