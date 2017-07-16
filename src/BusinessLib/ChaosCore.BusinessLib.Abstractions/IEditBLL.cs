using ChaosCore.ModelBase;

namespace ChaosCore.BusinessLib
{
    /// <summary>
    /// 可编辑BLL接口
    /// </summary>
    /// <typeparam name="TEntity">可编辑实体</typeparam>
    public interface IEditBLL<TEntity> : IBaseBLL
        where TEntity : BaseEntity
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Edit(TEntity entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Remove(long id);
    }
}
