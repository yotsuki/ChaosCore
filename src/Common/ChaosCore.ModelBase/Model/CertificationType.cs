using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase
{
    public class CertificationType : BaseGuidEntity, IBaseGuidEntity
    {
        public string TypeName { get; set; }
        public string Icon { get; set; }
    }
}
