using ChaosCore.ModelBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ChaosCore.RepositoryLib
{
    public abstract class Repository
    {
        public Guid RepositoryID { get; set; } = Guid.NewGuid();
        private IServiceProvider _serviceProvider = null;
        protected Dictionary<string, DbContextConfiguration> _mapDbContext = null;

        public Repository() { }
        public Repository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapDbContext = serviceProvider.GetService<IOptions<Dictionary<string, DbContextConfiguration>>>().Value;
        }
        //public virtual string ContextName { get; set; }
        private IDbContextStorage _dbStorage = null;
        public virtual IDbContextStorage DbStorage { get {
                return _dbStorage ?? (_dbStorage = _serviceProvider.GetService<IDbContextStorage>());
            }
            set {
                _dbStorage = value;
            }
        }
        protected abstract string GetContextName();
        public IChaosCoreDbContext GetContext()
        {
            return DbStorage.GetByKey(GetContextName());
        }

        protected DbSet<TEntity> GetDbSet<TEntity>()
            where TEntity : BaseEntity
        {
            var context = GetContext();
            foreach (PropertyInfo dbSet in context.GetType().GetProperties().Where(t => t.PropertyType.GetTypeInfo().IsGenericType && t.PropertyType.GetGenericTypeDefinition().Equals(typeof(DbSet<>)))) {
                Type[] entityType = dbSet.PropertyType.GetGenericArguments();
                if (typeof(TEntity) == entityType[0]) {
                    return dbSet.GetValue(context, null) as DbSet<TEntity>;
                }
            }
            return null;
        }
    }
    public class Repository<TEntity> : Repository, IRepository<TEntity>
        where TEntity : BaseEntity, new()
    {
        public Repository() { }
        public Repository(IServiceProvider serviceProvider):base(serviceProvider)
        {
        }
        protected override string GetContextName()
        {
            var entityname = typeof(TEntity).Name;
            var name = _mapDbContext.Where(c => c.Value.Entites != null && c.Value.Entites.Contains(entityname)).Select(kvp=>kvp.Key).FirstOrDefault();
            if(name == null) {
                name = _mapDbContext.Where(kvp => kvp.Value.Entites == null || !kvp.Value.Entites.Any()).Select(kvp=>kvp.Key).FirstOrDefault();
            }
            return name;
        }
        private DbSet<TEntity> _dbSet = null;
        protected DbSet<TEntity> DbSet {
            get {
                return _dbSet ?? (_dbSet = GetDbSet<TEntity>());
            }
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual int Count()
        {
            return DbSet.Count();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Count(predicate);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            DbSet.RemoveRange(DbSet.Where(predicate));
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        public virtual void Delete(params object[] value)
        {
            var entity = Activator.CreateInstance<TEntity>();
            var context = GetContext() as DbContext;
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();
            if(key.Properties.Count != value.Length) {
                throw new Exception("key count error");
            }
            
            for(var i = 0; i < value.Length; i++) {
                key.Properties[i].PropertyInfo.SetValue(entity, value[i]);
            }

            var entry = DbSet.Attach(entity);
            entry.State = EntityState.Deleted;
        }
        public virtual void DeleteRange(IEnumerable<TEntity> entitys)
        {
            DbSet.RemoveRange(entitys);
        }

        public virtual TEntity Find(params object[] value)
        {
            return DbSet.Find(value);
        }

        public virtual TEntity Find(bool proxy, params object[] value)
        {
            //var tmpproxy = GetContext().GetObjectContext().ContextOptions.ProxyCreationEnabled;
            //GetContext().GetObjectContext().ContextOptions.ProxyCreationEnabled = proxy;
            var result = DbSet.Find(value);
            //GetContext().GetObjectContext().ContextOptions.ProxyCreationEnabled = tmpproxy;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updater"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual TEntity AttachUpdate(Expression<Func<TEntity, TEntity>> updater, params object[] value)
        {
            var context = GetContext() as DbContext;
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();

            //var type = typeof(TEntity);
            //var entity = EntityKeyHelper.CreateEntity<TEntity>(EntityKeyHelper.GetEntityKey<TEntity>(value));
            //DbSet.Attach(entity);
            //var updateMemberExpr = (MemberInitExpression)updater.Body;
            //foreach(var m in updateMemberExpr.Bindings.Cast<MemberAssignment>()) {
            //    var pi = type.GetProperty(m.Member.Name);
            //    pi.SetValue(entity, ((ConstantExpression)m.Expression).Value, null);
            //}
            //return entity;
            return null;
        }

        public virtual EntityEntry<TEntity> Attach(TEntity entity)
        {
            return DbSet.Attach(entity);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<TEntity> GetQuery()
        {
            return DbSet.AsQueryable();
        }

        public virtual IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
        public virtual IQueryable<TEntity> GetQuery<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> path)
        {
            return DbSet.Where(predicate).Include(path);
        }
        public virtual IQueryable<TEntity> GetIncludeQuery(params string[] includepath)
        {
            var query = DbSet.AsQueryable();
            foreach(var path in includepath) {
                query = query.Include(path);
            }
            return query;
        }
        public bool IsModified(object entity)
        {
            //var context = GetContext() as DbContext;
            //var objectStateEntry = context.ChangeTracker..GetObjectStateEntry(entity);
            //return objectStateEntry.State == EntityState.Modified;
            return false;
        }
        public IEnumerable<EntityEntry> GetChangeEntries()
        {
            return GetContext().GetChangeEntries();
        }

        public void Reload(TEntity entity)
        {
            //GetContext().GetObjectContext().Refresh(RefreshMode.ClientWins, (object)entity);
        }

        //public IQueryable<TEntity> Reload(IEnumerable<TEntity> list, Expression<Func<TEntity, bool>> predicate)
        //{
        //    GetContext().GetObjectContext().Refresh(RefreshMode.ClientWins, list.Where(predicate));
        //}

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        public IEnumerable<TResult> SqlQuery<TResult>(string sql)
        {
            //return DbSet.SqlQuery(sql,);
            return null;
        }
        public virtual int UpdateBatch(Expression<Func<TEntity, bool>> wherePredicate, Expression<Func<TEntity, TEntity>> updater)
        {
            //var sqlgen = new SqlGenerate(DbSet.Where(wherePredicate));
            //string commandText = string.Format("Update {0} Set {1} Where {2}", sqlgen.Form, sqlgen.GetUpdateSet<TEntity>(updater), sqlgen.Where);
            //var Result = GetContext().GetObjectContext().ExecuteStoreCommand(commandText, sqlgen.GetUpdateParameters<TEntity>(updater));
            //return Result;
            return 0;
        }

        public virtual int DeleteBatch(Expression<Func<TEntity, bool>> wherePredicate)
        {
            //var sqlgen = new SqlGenerate(DbSet.Where(wherePredicate));
            //string commandText = string.Format("Delete From {0} Where {1}", sqlgen.Form, sqlgen.Where);
            //var Result = GetContext().GetObjectContext().ExecuteStoreCommand(commandText);
            //return Result;
            return 0;
        }

        public virtual IQueryable<TEntity> EqualWhere(Expression<Func<TEntity, TEntity>> wherePredicate)
        {
            //var whereBody = (MemberInitExpression)wherePredicate.Body;
            //var parameter = Expression.Parameter(typeof(TEntity), "tentity");
            //var whereMemberCollection = whereBody.Bindings.Cast<MemberAssignment>()
            //                        .Where(m => ExpressionHelper.GetValue(m.Expression) != null)
            //                        .Select(m => Expression.Equal(Expression.MakeMemberAccess(parameter, m.Member), m.Expression))
            //                        .ToArray();
            //var query = GetQuery();
            //if (whereMemberCollection.Length == 0) {
            //    return query;
            //} else {
            //    var expr = whereMemberCollection[0];
            //    for (int i = 1; i < whereMemberCollection.Length; i++) {
            //        expr = Expression.AndAlso(expr, whereMemberCollection[i]);
            //    }
            //    Expression<Func<TEntity, bool>> whereExpr = Expression.Lambda<Func<TEntity, bool>>(expr, parameter);
            //    return query.Where(whereExpr);
            //}
            return null;
        }

        IEnumerable<EntityEntry> IRepository<TEntity>.GetChangeEntries()
        {
            return GetContext().GetChangeEntries();
        }

        public TEntity GetByID<TKey>(TKey key)
        {
            System.Diagnostics.Debug.WriteLine(key);
            var entityEntity = GetContext().GetChangeEntries().FirstOrDefault(e => e.Entity is TEntity && ((IBaseEntity<TKey>)e.Entity).ID.Equals(key));
            if (entityEntity == null) {
                var entity = Activator.CreateInstance(typeof(TEntity)) as TEntity;
                if (entity is IBaseEntity<TKey>) {
                    ((IBaseEntity<TKey>)entity).ID = key;
                }
                GetContext().Unchanged(entity);
                return entity;
            } else {
                return entityEntity.Entity as TEntity;
            }
        }
    }


}
