using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.BusinessLib
{
    /// <summary>
    /// 业务Session
    /// 用户BusinessLib之间传值使用
    /// </summary>
    public interface IBusinessSession
    {
        long UserID { get; set; }

        string UserName { get; set; }
    }
}
