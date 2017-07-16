using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.CommonLib
{
    public enum NowMode
    {
        Now,
        UtcNow
    }
    public static class DateTimeHelper
    {
        public static NowMode Mode = NowMode.Now;
        public static DateTime GetNow()
        {
            if( Mode == NowMode.Now) {
                return DateTime.Now;
            }else if(Mode == NowMode.UtcNow) {
                return DateTime.UtcNow;
            } else {
                return DateTime.Now;
            }
        }
    }
}
