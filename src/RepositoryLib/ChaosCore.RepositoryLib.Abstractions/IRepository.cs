using ChaosCore.ModelBase;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace ChaosCore.RepositoryLib
{
    /// <summary>
    /// 基本操作接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IChaosRepository
                            where TEntity : BaseEntity
    {
        /// <summary>
        /// 获取IQueryable
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<TEntity> GetQuery();

        /// <summary>
        /// 根据条件获取IQueryable
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>IQueryable</returns>
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 带Include的查询
        /// </summary>
        /// <typeparam name="TProperty">Include属性类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="path">include表达式</param>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> path);

        /// <summary>
        /// 带Include的查询
        /// </summary>
        /// <param name="includepath">include路径</param>
        /// <returns>IQueryable</returns>
        IQueryable<TEntity> GetIncludeQuery(params string[] includepath);

        /// <summary>
        /// 根据主键属性得到实体
        /// </summary>
        /// <param name="value">主键属性值</param>
        /// <returns>实体</returns>
        TEntity Find(params object[] value);
        /// <summary>
        /// 根据主键获取一个空的实体(只有主键)
        /// 主要用于建立关联关系
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity GetByID<TKey>(TKey key);
        /// <summary>
        /// 根据主键属性得到实体
        /// </summary>
        /// <param name="proxy">是否开启代理</param>
        /// <param name="value">主键属性值</param>
        /// <returns>实体</returns>
        TEntity Find(bool proxy, params object[] value);
        EntityEntry<TEntity> Attach(TEntity entity);
        TEntity AttachUpdate(Expression<Func<TEntity, TEntity>> updater, params object[] value);
        /// <summary>
        /// 根据条件获取唯一的实体
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>实体</returns>
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获取第一条实体
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>实体</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据条件获取列表
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>实体列表</returns>
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Add(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Delete(TEntity entity);
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="value">key</param>
        void Delete(params object[] value);
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 删除实体列表
        /// </summary>
        /// <param name="entitys">实体列表</param>
        void DeleteRange(IEnumerable<TEntity> entitys);
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns>总数</returns>
        int Count();

        /// <summary>
        /// 根据条件获取总数
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <returns>总数</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        ///// <summary>
        ///// 执行SQL
        ///// </summary>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //IEnumerable<TResult> SqlQuery<TResult>(string sql);
        /// <summary>
        /// 判断实体是否已被修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>是否已被修改</returns>
        bool IsModified(object entity);
        /// <summary>
        /// 重新加载实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Reload(TEntity entity);

        ///// <summary>
        ///// 要Reload
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //IQueryable<TEntity> Reload(IEnumerable<TEntity> list, Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 批量更新
        /// 该操作将直接提交不等待SaveChanges调用
        /// </summary>
        /// <param name="wherePredicate"></param>
        /// <param name="updater"></param>
        /// <returns></returns>
        int UpdateBatch(Expression<Func<TEntity, bool>> wherePredicate, Expression<Func<TEntity, TEntity>> updater);
        /// <summary>
        /// 批量删除
        /// 该操作将直接提交不等待SaveChanges调用
        /// </summary>
        /// <param name="wherePredicate"></param>
        /// <returns></returns>
        int DeleteBatch(Expression<Func<TEntity, bool>> wherePredicate);

        IQueryable<TEntity> EqualWhere(Expression<Func<TEntity, TEntity>> wherePredicate);

        IEnumerable<EntityEntry> GetChangeEntries();
    }
}
