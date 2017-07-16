using System;
using System.Collections.Generic;
using System.Text;

namespace ChaosCore.CommonLib.EnumType
{
    public enum Sex
    {
        /// <summary>
        /// 未知的性别
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 男
        /// </summary>
        Male,
        /// <summary>
        /// 女
        /// </summary>
        Female,
        /// <summary>
        /// 女性改（变）为男性
        /// </summary>
        FamaleToMale = 5,
        /// <summary>
        /// 男性改（变）为女性 
        /// </summary>
        MaleToFemale = 6,
        /// <summary>
        /// 未说明的性别
        /// </summary>
        NoSet = 9,
    }
}
