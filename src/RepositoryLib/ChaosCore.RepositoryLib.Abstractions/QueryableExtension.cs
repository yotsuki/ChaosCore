using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ChaosCore.RepositoryLib
{
    public static class QueryableExtension
    {
        public static T AddIfNotExist<T>(this DbSet<T> query, Expression<Func<T, bool>> predicate, T entity,params string[] includes) where T : class
        {
            if (query.Any(predicate)) {
                var q = query.AsQueryable();
                foreach(var include  in includes) {
                    q = query.Include(include);
                }
                return q.FirstOrDefault(predicate);
            } else {
                query.Add(entity);
                return entity;
            }
        }
    }
}
