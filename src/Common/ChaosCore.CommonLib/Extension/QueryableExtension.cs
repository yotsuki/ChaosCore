using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ChaosCore.CommonLib.Extension
{
    public static class QueryableExtension
    {
        public static IQueryable<T> Paged<T>(this IQueryable<T> query, int pageindex, int pagesize)
        {
            if (pageindex > 1) {
                query = query.Skip((pageindex - 1) * pagesize);
            };
            return query.Take(pagesize);
        }
        public static IQueryable<T> Paged<T>(this IQueryable<T> query, PageModel page,bool bPlus =false)
        {
            if(page == null) {
                return query;
            }
            if (page.Index > 1) {
                query = query.Skip((page.Index - 1) * page.Size);
            };
            var size = bPlus?page.Size+1:page.Size;
            return query.Take(size);
        }

        public static IQueryable<T> Paged<T, TKey>(this IQueryable<T> query, PageModel page , Expression<Func<T, TKey>> ordeyBySelector, bool bPlus = false)
        {
            if (page.Index > 1) {
                query = query.OrderBy(ordeyBySelector).Skip((page.Index - 1) * page.Size);
            };
            var size = bPlus ? page.Size + 1 : page.Size;
            return query.OrderBy(ordeyBySelector).Take(size);
        }
        private static MethodInfo s_methodOrderByASC = null;
        private static MethodInfo s_methodOrderByDESC = null;
        public static IQueryable<T> Paged<T>(this IQueryable<T> query, PageModel page, string ordername,bool desc = false, bool bPlus = false)
        {
            var size = bPlus ? page.Size + 1 : page.Size;
            Type type = typeof(T);
            var property = type.GetProperty(ordername);
            if (property == null) {
                property = type.GetProperties().FirstOrDefault();
            }
            MethodInfo ordermethod = null;
            if (!desc) {
                if (s_methodOrderByASC == null) {
                    s_methodOrderByASC = typeof(Queryable).GetMethods().Where(m => m.Name == "OrderBy").FirstOrDefault();
                }
                ordermethod = s_methodOrderByASC.MakeGenericMethod(type, property.PropertyType);
            } else {
                if (s_methodOrderByDESC == null) {
                    s_methodOrderByDESC = typeof(Queryable).GetMethods().Where(m => m.Name == "OrderByDescending").FirstOrDefault();
                }
                ordermethod = s_methodOrderByDESC.MakeGenericMethod(type, property.PropertyType);
            }
            var parameterExpression = Expression.Parameter(type, "q");
            var exprProperty = Expression.Property(parameterExpression, ordername);
            var orderExpression = Expression.Lambda(exprProperty, (ParameterExpression)parameterExpression);

            var orderQuery = (IQueryable<T>)ordermethod.Invoke(null, new object[] { query, orderExpression });
            if (page.Index > 1) {
                orderQuery = orderQuery.Skip((page.Index - 1) * page.Size);
            };

            return orderQuery.Take(size);
        }

        public static IQueryable<T> Order<T>(this IQueryable<T> query, string ordername, bool desc = false)
        {
            Type type = typeof(T);
            var property = type.GetProperty(ordername);
            if (property == null) {
                property = type.GetProperties().FirstOrDefault();
            }
            MethodInfo ordermethod = null;
            if (!desc) {
                if (s_methodOrderByASC == null) {
                    s_methodOrderByASC = typeof(Queryable).GetMethods().Where(m => m.Name == "OrderBy").FirstOrDefault();
                }
                ordermethod = s_methodOrderByASC.MakeGenericMethod(type, property.PropertyType);
            } else {
                if (s_methodOrderByDESC == null) {
                    s_methodOrderByDESC = typeof(Queryable).GetMethods().Where(m => m.Name == "OrderByDescending").FirstOrDefault();
                }
                ordermethod = s_methodOrderByDESC.MakeGenericMethod(type, property.PropertyType);
            }
            var parameterExpression = Expression.Parameter(type, "q");
            var exprProperty = Expression.Property(parameterExpression, ordername);
            var orderExpression = Expression.Lambda(exprProperty, (ParameterExpression)parameterExpression);

            var orderQuery = (IQueryable<T>)ordermethod.Invoke(null, new object[] { query, orderExpression });

            return orderQuery;
        }
    }
}
