using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase
{
    /// <summary>
    /// ErrorCode定义
    /// </summary>
    public class ErrorCode
    {
        /// <summary>
        /// 正常
        /// </summary>
        public const byte NORMAL = 0;
        /// <summary>
        /// ID错误
        /// </summary>
        public const byte ID_ERROR = 1;
        /// <summary>
        /// ID不存在
        /// </summary>
        public const byte ID_NOT_FOUND = 2;
        /// <summary>
        /// 参数为空
        /// </summary>
        public const byte PARAM_NULL = 3;
        /// <summary>
        /// 用户未登录
        /// </summary>
        public const byte USER_NOT_LOGIN = 8;
        /// <summary>
        /// 用户自定义错误
        /// 64-127 Custom error code
        /// </summary>
        public const byte USERCUSTOM    = 64;
        /// <summary>
        /// 用户自定义警告
        /// 128-191 Custom Warning code 
        /// </summary>
        public const byte USERCUSTOMWARNING = 128;
        /// <summary>
        /// 并发冲突
        /// </summary>
        public const byte OPTIMISTIC_CONCURRENCY = 0xfe;
        /// <summary>
        /// 不明错误
        /// </summary>
        public const byte UNKOWN_ERROR = 0xff;
    }
}
