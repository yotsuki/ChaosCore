using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase
{
    public interface ICreateInfo
    {
        DateTime CreateTime { get; set; }
        long Creator { get; set; }
    }
}
