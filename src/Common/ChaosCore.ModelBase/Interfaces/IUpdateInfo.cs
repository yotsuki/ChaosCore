using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase
{
    public interface IUpdateInfo
    {
        long UpdateUserID { get; set; }


        DateTime LastUpdateTime { get; set; }
    }
}
