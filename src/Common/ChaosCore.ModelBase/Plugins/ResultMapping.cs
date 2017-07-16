using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ChaosCore.ModelBase.Plugins
{

    public class ResultMapping<T> where T : BaseEntity
    {
        private static Expression<Func<T, string>> m_defualtText = t => "";
        protected Dictionary<string, Func<T, string>> m_mapKeyHtml = new Dictionary<string, Func<T, string>>();
        protected Dictionary<string, Expression<Func<T, string>>> m_mapKeyText = new Dictionary<string, Expression<Func<T, string>>>();
        protected Dictionary<string, Func<IQueryable<T>, string, IQueryable<T>>> m_mapWhere = new Dictionary<string, Func<IQueryable<T>, string, IQueryable<T>>>();
        protected Dictionary<string, MemberInfo> m_mapMembers = new Dictionary<string, MemberInfo>();
        public ResultMapping<T> SetHtml(string name, Func<T, string> html)
        {
            m_mapKeyHtml[name] = html;
            return this;
        }
        public ResultMapping<T> SetText(string name, Expression<Func<T, string>> text, bool setHtml = true)
        {
            m_mapKeyText[name] = text;
            if (setHtml) {
                SetHtml(name, text.Compile());
            }
            return this;
        }
        public ResultMapping<T> SetWhere(string name, Func<IQueryable<T>, string, IQueryable<T>> where)
        {
            m_mapWhere[name] = where;
            return this;
        }
        public virtual IQueryable<T> Where(IQueryable<T> query, string name , string value)
        {
            if (m_mapWhere.ContainsKey(name)) {
                query = m_mapWhere[name].Invoke(query, value);
            }
            return query;
        }
        public virtual IQueryable<T> Include(IQueryable<T> query)
        {
            return query;
        }
        public virtual string GetHtml(T source,string name)
        {
            if (m_mapKeyHtml.ContainsKey(name)) {
                return m_mapKeyHtml[name](source);
            }else {
                return GetOtherHtml(source,name);
            }
        }

        public virtual string GetOtherHtml(T source, string name)
        {
            return null;
        }

        public virtual string GetText(T source, string name)
        {
            if (m_mapKeyText.ContainsKey(name)) {
                return m_mapKeyText[name].Compile().Invoke(source);
            } else {
                return GetOtherText(source, name);
            }
        }
        public virtual string GetOtherText(T source, string name)
        {
            return null;
        }
        public virtual Expression<Func<T, string>> GetTextFunc(string name)
        {
            if (m_mapKeyText.ContainsKey(name)) {
                return m_mapKeyText[name];
            } else {
                return m_defualtText;
            }
        }
        public NewExpression NewExpression { get; set; }
        public ResultMapping<T> MemberBaseEntity(bool lower = false)
        {
            var type = typeof(T);
            if (NewExpression == null) {
                NewExpression = Expression.New(type);
            }
            foreach (var property in type.GetTypeInfo().GetProperties(BindingFlags.Public| BindingFlags.Instance)) {
                var name = property.Name;
                if (lower) {
                    name = name.ToLower();
                }
                m_mapMembers.Add(name, property);
            }
            return this;
        }

        public virtual IQueryable<T> Select(IQueryable<T> query, IEnumerable<string> names = null)
        {
            var parameter = Expression.Parameter(typeof(T), "e");
            List<MemberBinding> MemberBindings = new List<MemberBinding>();
            foreach (var member in m_mapMembers) {
                if(names != null && !names.Contains(member.Key)) {
                    continue;
                }
                var valueExpr = Expression.MakeMemberAccess(parameter, member.Value);
                MemberBindings.Add(Expression.Bind(member.Value, valueExpr));
            }
            var memberInitExpr = Expression.MemberInit(NewExpression, MemberBindings.ToArray());
            var lambda = Expression.Lambda(memberInitExpr, parameter);
            return query.Select((Expression<Func<T, T>>)lambda);
        }

        public IEnumerable<T> Query(QuickQueryModel model, IQueryable<T> items) 
        {
            if (model == null) {
                return null;
            }
            var result = new QueryResultModel();
            result.Draw = model.Draw;
            result.RecordsTotal = items.Select(e => e.CreateTime).Count();
            var filtereditems = items;
            foreach (var filter in model.Columns.Where(c => c.Searchable && !string.IsNullOrEmpty(c.Search.Value))) {
                filtereditems = Where(filtereditems, filter.Name, filter.Search.Value);
            }
            if (model.Filters != null) {
                foreach (var filter in model.Filters) {
                    filtereditems = Where(filtereditems, filter.Column, filter.Value);
                }
            }
            result.RecordsFiltered = filtereditems.Select(e => e.CreateTime).Count();
            foreach (var order in model.Order) {
                var ordername = order.Column;
                filtereditems = (order.Dir == OrderDir.asc) ? filtereditems.OrderBy(GetTextFunc(ordername)) : filtereditems.OrderByDescending(GetTextFunc(ordername));
            }
            filtereditems = Select(filtereditems, model.Columns.Select(c=>c.Name));

            var pageditems = (model.Length == -1 ? filtereditems : (model.Start == 0 ? filtereditems : filtereditems.Skip(model.Start)).Take(model.Length)).ToArray();

            return pageditems;
        }
    }
}