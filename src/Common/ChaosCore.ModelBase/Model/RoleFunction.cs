using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChaosCore.ModelBase
{
    public class RoleFunction
    {
        [Key, ForeignKey("Role")]
        public long RoleID { get; set; }
        [Key, ForeignKey("Function")]
        public long FunctionID { get; set; }

        public virtual Role Role { get; set; }
        public virtual Function Function { get; set; }
    }
}
