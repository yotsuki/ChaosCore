using ChaosCore.ModelBase;
using System;
using System.Linq.Expressions;

namespace ChaosCore.BusinessLib
{
    public interface IUpdateBLL<TEntity> : IBaseBLL<TEntity>
        where TEntity : BaseEntity
    {
        Result<TEntity> Update(TEntity entity, bool bCreate);

        Result<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    }
}
