using ChaosCore.ModelBase;
using System;

namespace ChaosCore.BusinessLib
{
    /// <summary>
    /// BLL基接口
    /// </summary>
    public interface IBaseBLL:IDisposable
    {
        long UserID { get; set; }
        string UserName { get; set; }
        /// <summary>
        /// 用户Session
        /// </summary>
        IBusinessSession Session { get; set; }
        IServiceProvider ServiceProvider { get; set; }
    }

    public interface IBaseBLL<TEntity> : IBaseBLL
        where TEntity : BaseEntity
    {

    }
}
