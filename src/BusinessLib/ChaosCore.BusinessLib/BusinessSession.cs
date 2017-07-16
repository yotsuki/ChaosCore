using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.BusinessLib
{
    public class BusinessSession : IBusinessSession
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
    }
}
