using System.ComponentModel.DataAnnotations;

namespace ChaosCore.ModelBase
{
    public class SysConfig: BaseLongEntity, IBaseLongEntity
    {
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Section { get; set; }
        [StringLength(1024)]
        public string Value { get; set; }
        [StringLength(1024)]
        public string Description { get; set; }
        [StringLength(1024)]
        public string Verification { get; set; }
    }
}
