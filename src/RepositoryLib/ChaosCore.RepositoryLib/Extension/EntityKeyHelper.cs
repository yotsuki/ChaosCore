using ChaosCore.ModelBase;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ChaosCore.RepositoryLib.Extension
{
    public static class EntityKeyHelper
    {
        private static Dictionary<string, string[]> _dictEntityKeyNames = new Dictionary<string, string[]>();
        private static string[] GetKeyNames<TEntity>() where TEntity : BaseEntity
        {

            var type = typeof(TEntity);
            if (!_dictEntityKeyNames.ContainsKey(type.FullName)) {
                var list = new List<string>();
                var arr = type.GetProperties().Where(p => p.IsDefined(typeof(KeyAttribute), true)).ToArray();
                if (arr == null || arr.Length == 0) {
                    list.Add("ID");
                } else {
                    foreach (var p in arr) {
                        list.Add(p.Name);
                    }
                }
                _dictEntityKeyNames[type.FullName] = list.ToArray();
            }
            return _dictEntityKeyNames[type.FullName];

        }
        public static IKey GetEntityKey<TEntity>(params object[] ids) where TEntity : BaseEntity, new()
        {
            var keynames = GetKeyNames<TEntity>();
            if (keynames.Length != ids.Length) {
                return null;
            }
            
            //var members = new List<EntityKeyMember>();
            //for (int i = 0; i < ids.Length; i++) {
            //    members.Add(new EntityKeyMember() { Key = keynames[i], Value = ids[i] });
            //}
            //var entitykey = new EntityKey("", members);
            return null;
        }
        public static TEntity CreateEntity<TEntity>(IKey key) where TEntity : BaseEntity, new()
        {
            var entity = new TEntity();
            var type = typeof(TEntity);
            foreach(var member in key.GetAnnotations()) {
                var pi =  type.GetProperty(member.Name);
                pi.SetValue(entity, member.Value,null);
            }
            return entity;
        }
        public static void SetEntityKey<TEntity>(TEntity entity, params object[] ids) where TEntity : BaseEntity, new()
        {

        }
    }
}
