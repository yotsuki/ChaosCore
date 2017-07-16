using ChaosCore.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Data;
using ChaosCore.CommonLib;
using System.IO;
using ChaosCore.ModelBase.Attributes;

namespace ChaosCore.ModelBase.Extensions
{
    public static class BaseEntityExtensions
    {
        public static void UpdateDateTime(this IUpdateInfo entity)
        {
            entity.LastUpdateTime = DateTimeHelper.GetNow();
        }
        public static T ConvertTo<T>(this BaseEntity baseEntity)
        {
            Type type = baseEntity.GetType();
            T instance = (T)System.Activator.CreateInstance(typeof(T));
            foreach (PropertyInfo pObj in type.GetProperties()) {
                if (pObj.GetSetMethod() != null) {
                    PropertyInfo f = instance.GetType().GetProperty(pObj.Name);
                    f.SetValue(instance, pObj.GetValue(baseEntity, null), null);
                }
            }

            return instance;
        }

        public static T ConvertTo<T>(this BaseEntity baseEntity, params string[] notCopyProperty)
        {
            Type type = baseEntity.GetType();
            T instance = (T)System.Activator.CreateInstance(typeof(T));
            foreach (PropertyInfo pObj in type.GetProperties()) {
                if (notCopyProperty.Contains(pObj.Name.ToLower())) continue;
                if (pObj.GetSetMethod() != null) {
                    PropertyInfo f = instance.GetType().GetProperty(pObj.Name);
                    f.SetValue(instance, pObj.GetValue(baseEntity, null), null);
                }
            }

            return instance;
        }

        public static T CopyTo<T>(this BaseEntity baseEntity, params string[] propertyNames)
        {
            Type type = baseEntity.GetType();
            T instance = (T)System.Activator.CreateInstance(typeof(T));
            foreach (var item in propertyNames) {
                PropertyInfo f = instance.GetType().GetProperty(item);
                PropertyInfo pObj = type.GetProperty(item);
                if (pObj == null) {
                    f.SetValue(instance, TypeConvertor.GetDefault(f.PropertyType), null);
                } else {
                    object value = pObj.GetValue(baseEntity, null);
                    if (value == null)
                        f.SetValue(instance, TypeConvertor.GetDefault(f.PropertyType), null);
                    else
                        f.SetValue(instance, Convert.ChangeType(value, Nullable.GetUnderlyingType(f.PropertyType) ?? f.PropertyType, System.Globalization.CultureInfo.CurrentCulture), null);
                }
            }
            return instance;
        }

        public static T CopyTo<T>(this BaseEntity baseEntity, ref T newEntity, params string[] propertyNames)
        {
            Type type = baseEntity.GetType();
            foreach (var item in propertyNames) {
                PropertyInfo f = newEntity.GetType().GetProperty(item);
                PropertyInfo pObj = type.GetProperty(item);
                if (pObj == null) {
                    f.SetValue(newEntity, TypeConvertor.GetDefault(f.PropertyType), null);
                } else {
                    object value = pObj.GetValue(baseEntity, null);
                    if (value == null)
                        f.SetValue(newEntity, TypeConvertor.GetDefault(f.PropertyType), null);
                    else
                        f.SetValue(newEntity, Convert.ChangeType(value, Nullable.GetUnderlyingType(f.PropertyType) ?? f.PropertyType, System.Globalization.CultureInfo.CurrentCulture), null);
                }
            }
            return newEntity;
        }

        public static T CopyTo<T>(this object baseEntity, ref T newEntity, params string[] propertyNames)
        {
            Type type = baseEntity.GetType();
            foreach (var item in propertyNames) {
                if (string.IsNullOrEmpty(item)) {
                    continue;
                }
                PropertyInfo f = newEntity.GetType().GetProperty(item);
                PropertyInfo pObj = type.GetProperty(item);
                if (pObj == null) {
                    f.SetValue(newEntity, TypeConvertor.GetDefault(f.PropertyType), null);
                } else {
                    object value = pObj.GetValue(baseEntity, null);
                    if (value == null)
                        f.SetValue(newEntity, TypeConvertor.GetDefault(f.PropertyType), null);
                    else
                        f.SetValue(newEntity, Convert.ChangeType(value, Nullable.GetUnderlyingType(f.PropertyType) ?? f.PropertyType, System.Globalization.CultureInfo.CurrentCulture), null);
                }
            }
            return newEntity;
        }

        public static T CopyTo<T>(this object baseEntity, params string[] propertyNames)
        {
            Type type = baseEntity.GetType();
            var newEntity = (T)Activator.CreateInstance(typeof(T));
            foreach (var item in propertyNames) {
                PropertyInfo f = newEntity.GetType().GetProperty(item.Trim());
                PropertyInfo pObj = type.GetProperty(item);
                if (pObj == null) {
                    f.SetValue(newEntity, TypeConvertor.GetDefault(f.PropertyType), null);
                } else {
                    object value = pObj.GetValue(baseEntity, null);
                    if (value == null)
                        f.SetValue(newEntity, TypeConvertor.GetDefault(f.PropertyType), null);
                    else
                        f.SetValue(newEntity, Convert.ChangeType(value, Nullable.GetUnderlyingType(f.PropertyType) ?? f.PropertyType, System.Globalization.CultureInfo.CurrentCulture), null);
                }
            }
            return newEntity;
        }
        public static bool CopyFrom(this BaseEntity baseEntity, Dictionary<string, string> dict)
        {
            if (baseEntity == null) {
                return false;
            }

            var entitytype = baseEntity.GetType();
            //Dictionary<string, string> lstNames = new Dictionary<string, string>();//key:实体属性名。name：表示名
            List<string> names = new List<string>();
            foreach (var p in entitytype.GetProperties()) {
                if (p.PropertyType.FullName.Contains("ICollection")) {
                    continue;
                }
                if (p.PropertyType.GetTypeInfo().IsSubclassOf(typeof(BaseEntity))) {
                    continue;
                }
                var jsonignore = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(JsonIgnoreAttribute));
                if (jsonignore != null) {
                    continue;
                }
                names.Clear();
                names.Add(p.Name);
                string name = p.Name;
                string key = name;
                var displaycolumn = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(DisplayColumnAttribute));
                if (displaycolumn != null) {
                    name = ((DisplayColumnAttribute)displaycolumn).DisplayColumn;
                } else {
                    displaycolumn = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(DisplayAttribute));
                    if (displaycolumn != null) {
                        name = ((DisplayAttribute)displaycolumn).Name;
                    }
                }
                names.Insert(1, name);
                string strValue = string.Empty;
                foreach (var n in names) {
                    if (dict.ContainsKey(n)) {
                        strValue = dict[n];
                        break;
                    }
                }
                if (string.IsNullOrEmpty(strValue)) {
                    continue;
                }
                var type = p.PropertyType;
                if (type.GetTypeInfo().IsGenericType) {
                    type = type.GetGenericArguments()[0];
                }
                if (type == typeof(string)) {
                    p.SetValue(baseEntity, strValue, null);
                } else if (type == typeof(long)) {
                    long value = 0;
                    if (long.TryParse(strValue, out value)) {
                        p.SetValue(baseEntity, value, null);
                    }
                } else if (type == typeof(int)) {
                    int value = 0;
                    if (int.TryParse(strValue, out value)) {
                        p.SetValue(baseEntity, value, null);
                    }
                } else if (type == typeof(byte)) {
                    byte value = 0;
                    if (byte.TryParse(strValue, out value)) {
                        p.SetValue(baseEntity, value, null);
                    }
                } else if (type == typeof(short)) {
                    short value = 0;
                    if (short.TryParse(strValue, out value)) {
                        p.SetValue(baseEntity, value, null);
                    }
                } else if (type == typeof(bool)) {
                    bool value = false;
                    if (bool.TryParse(strValue, out value)) {
                        p.SetValue(baseEntity, value, null);
                    }
                } else if (type == typeof(decimal)) {
                    decimal value = 0;
                    if (decimal.TryParse(strValue, out value)) {
                        p.SetValue(baseEntity, value, null);
                    }
                } else if (type == typeof(float)) {
                    float value = 0;
                    if (float.TryParse(strValue, out value)) {
                        p.SetValue(baseEntity, value, null);
                    }
                } else if (type == typeof(double)) {
                    double value = 0;
                    if (double.TryParse(strValue, out value)) {
                        p.SetValue(baseEntity, value, null);
                    }
                } else if (type == typeof(DateTime)) {
                    DateTime value = DateTime.MinValue;
                    if (DateTime.TryParse(strValue, out value)) {
                        p.SetValue(baseEntity, value, null);
                    }
                } else if (type.GetTypeInfo().IsEnum) {
                    var value = Enum.Parse(type, strValue, true);
                    p.SetValue(baseEntity, value, null);
                }
            }
            return true;
        }
        public static bool CopyFrom(this BaseEntity baseEntity, object origonEntity, bool bSkipNull = true)
        {
            bool bChanged = false;
            var baseType = baseEntity.GetType();
            foreach (var property in origonEntity.GetType().GetProperties()) {
                var propertyName = property.Name;
                var baseProperty = baseType.GetProperties().FirstOrDefault(p => p.Name == propertyName);
                if (baseProperty == null) {
                    continue;
                }
                if (!property.PropertyType.IsAssignableFrom(baseProperty.PropertyType)) {
                    continue;
                }
                var newValue = property.GetValue(origonEntity, null);
                if (bSkipNull && newValue == null) {
                    continue;
                }
                var oldValue = baseProperty.GetValue(baseEntity, null);
                if (newValue == oldValue || newValue.Equals(oldValue)) {
                    continue;
                }
                if (property.PropertyType.GetTypeInfo().IsGenericType
                                && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) {
                    var instance = Activator.CreateInstance(property.PropertyType, newValue);
                    baseProperty.SetValue(baseEntity, instance, null);
                } else {
                    baseProperty.SetValue(baseEntity, newValue, null);
                }
                bChanged = true;
            }
            return bChanged;
        }

        public static bool Compare(this BaseEntity baseEntity, BaseEntity origonEntity, params string[] propertys)
        {
            if (baseEntity == null && origonEntity == null) {
                return true;
            }
            if (baseEntity == null && origonEntity != null || baseEntity != null && origonEntity != null) {
                return false;
            }
            Type type1 = baseEntity.GetType();
            Type type2 = origonEntity.GetType();
            foreach (var item in propertys) {
                PropertyInfo pi1 = type1.GetProperty(item);
                PropertyInfo pi2 = type2.GetProperty(item);
                if (pi1 == null && pi2 != null || pi1 != null && pi2 != null) {
                    return false;
                }
                if (pi1 == null && pi2 == null) {
                    continue;
                }
                var v1 = pi1.GetValue(baseEntity, null);
                var v2 = pi2.GetValue(origonEntity, null);
                if (v1 != v2) {
                    return false;
                }
            }

            return true;
        }
        public static object GetPropertyValue(this BaseEntity baseEntity, string propertyName)
        {
            var type = baseEntity.GetType();
            var property = type.GetProperties().FirstOrDefault(p => p.Name == propertyName);
            if (property == null) {
                return null;
            }

            return property.GetValue(baseEntity, null);
        }
        public static string ToXml(this BaseEntity baseEntity)
        {
            XmlSerializer seri = new XmlSerializer(baseEntity.GetType());
            string xml = string.Empty;
            using (var stream = new MemoryStream()) {
                seri.Serialize(stream, baseEntity);
                stream.Seek(0, SeekOrigin.Begin);
                var sr = new StreamReader(stream);
                xml = sr.ReadToEnd();
            }
            return xml;
        }

        public static void FormDictList<T>(this List<T> lstBaseEntity, IEnumerable<Dictionary<string, string>> lstDict, bool newID = true) where T : BaseEntity
        {
            if (lstBaseEntity == null) {
                return;
            }
            lstBaseEntity.Clear();
            var entitytype = typeof(T);
            Dictionary<string, string> lstNames = new Dictionary<string, string>();
            foreach (var p in entitytype.GetProperties()) {
                string name = p.Name;
                string key = name;
                var displaycolumn = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(DisplayColumnAttribute));
                if (displaycolumn != null) {
                    name = ((DisplayColumnAttribute)displaycolumn).DisplayColumn;
                } else {
                    displaycolumn = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(DisplayAttribute));
                    if (displaycolumn != null) {
                        name = ((DisplayAttribute)displaycolumn).Name;
                    }
                }
                lstNames.Add(name, key);
            }
            long id = 0;
            foreach (var dict in lstDict) {
                var entity = Activator.CreateInstance<T>();
                id++;
                foreach (var kvp in lstNames) {
                    if (dict.ContainsKey(kvp.Key)) {
                        var strValue = dict[kvp.Key];
                        var propType = entitytype.GetProperty(kvp.Value);
                        if (propType.PropertyType == typeof(string)) {
                            propType.SetValue(entity, strValue, null);
                        } else if (propType.PropertyType == typeof(decimal?)) {
                            decimal outValue = 0;
                            propType.SetValue(entity, decimal.TryParse(strValue, out outValue) ? outValue : new decimal?(), null);
                        } else if (propType.PropertyType == typeof(int?)) {
                            int outValue = 0;
                            propType.SetValue(entity, int.TryParse(strValue, out outValue) ? outValue : new int?(), null);
                        } else if (propType.PropertyType == typeof(long?)) {
                            long outValue = 0;
                            propType.SetValue(entity, long.TryParse(strValue, out outValue) ? outValue : new long?(), null);
                        } else if (propType.PropertyType == typeof(bool?)) {
                            bool outValue = false;
                            propType.SetValue(entity, bool.TryParse(strValue, out outValue) ? outValue : new bool?(), null);
                        } else if (propType.PropertyType == typeof(DateTime?)) {
                            DateTime outValue = DateTime.Now;
                            propType.SetValue(entity, DateTime.TryParse(strValue, out outValue) ? outValue : new DateTime?(), null);
                        } else if (propType.PropertyType == typeof(decimal)) {
                            decimal outValue = 0;
                            propType.SetValue(entity, decimal.TryParse(strValue, out outValue) ? outValue : 0, null);
                        } else if (propType.PropertyType == typeof(int)) {
                            int outValue = 0;
                            propType.SetValue(entity, int.TryParse(strValue, out outValue) ? outValue : 0, null);
                        } else if (propType.PropertyType == typeof(long)) {
                            long outValue = 0;
                            propType.SetValue(entity, long.TryParse(strValue, out outValue) ? outValue : 0, null);
                        } else if (propType.PropertyType == typeof(DateTime)) {
                            DateTime outValue = DateTime.Now;
                            propType.SetValue(entity, DateTime.TryParse(strValue, out outValue) ? outValue : DateTime.Now, null);
                        } else if (propType.PropertyType == typeof(bool)) {
                            bool outValue = false;
                            propType.SetValue(entity, bool.TryParse(strValue, out outValue) ? outValue : false, null);
                        }
                    }
                }
                if (newID) {
                    var IDType = entitytype.GetProperty("ID");
                    if (IDType.PropertyType == typeof(long) || IDType.PropertyType == typeof(int)) {
                        IDType.SetValue(entity, id, null);
                    } else if (IDType.PropertyType == typeof(Guid)) {
                        IDType.SetValue(entity, Guid.NewGuid(), null);
                    }
                }
                lstBaseEntity.Add(entity);
            }
        }
        //public static DataTable ToDataTable(this IEnumerable<BaseEntity> lstBaseEntity)
        //{
        //    if (lstBaseEntity == null) {
        //        return null;
        //    }

        //    var entitytype = lstBaseEntity.GetType().GetGenericArguments()[0];
        //    var dataTable = new DataTable(entitytype.Name);
        //    Dictionary<string, string> lstNames = new Dictionary<string, string>();//key:实体属性名。name：表示名
        //    foreach (var p in entitytype.GetProperties()) {
        //        if (p.PropertyType.FullName.Contains("ICollection")) {
        //            continue;
        //        }
        //        var jsonignore = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(JsonIgnoreAttribute));
        //        if (jsonignore != null) {
        //            continue;
        //        }
        //        string name = p.Name;
        //        string key = name;
        //        var displaycolumn = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(DisplayColumnAttribute));
        //        if (displaycolumn != null) {
        //            name = ((DisplayColumnAttribute)displaycolumn).DisplayColumn;
        //        } else {
        //            displaycolumn = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(DisplayAttribute));
        //            if (displaycolumn != null) {
        //                name = ((DisplayAttribute)displaycolumn).Name;
        //            }
        //        }
        //        lstNames.Add(key, name);
        //        var type = p.PropertyType;
        //        if (type.IsGenericType) {
        //            type = type.GetGenericArguments()[0];
        //        }
        //        dataTable.Columns.Add(new DataColumn(name, type));
        //    }

        //    foreach (var d in lstBaseEntity) {
        //        DataRow row = dataTable.NewRow();
        //        foreach (var name in lstNames) {
        //            var value = d.GetPropertyValue(name.Key);
        //            row[name.Value] = value ?? DBNull.Value;
        //        }
        //        dataTable.Rows.Add(row);
        //    }

        //    return dataTable;
        //}
        public static IEnumerable<Dictionary<string, string>> ToDictionary(this IEnumerable<BaseEntity> lstBaseEntity)
        {
            if (lstBaseEntity == null) {
                return null;
            }
            if (!lstBaseEntity.Any()) {
                return null;
            }
            var entitytype = lstBaseEntity.First().GetType();
            var datalist = new List<Dictionary<string, string>>();
            Dictionary<string, string> lstNames = new Dictionary<string, string>();//key:实体属性名。name：表示名
            Dictionary<string, StringFormatAttribute> lstFormat = new Dictionary<string, StringFormatAttribute>();//key:实体属性名。name：Format
            foreach (var p in entitytype.GetProperties()) {
                if (p.PropertyType.FullName.Contains("ICollection")) {
                    continue;
                }
                var jsonignore = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(JsonIgnoreAttribute));
                if (jsonignore != null) {
                    continue;
                }
                string name = p.Name;
                string key = name;
                if (lstNames.ContainsKey(key) && p.DeclaringType != entitytype) {
                    continue;
                }
                var displaycolumn = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(DisplayColumnAttribute));
                if (displaycolumn != null) {
                    name = ((DisplayColumnAttribute)displaycolumn).DisplayColumn;
                } else {
                    displaycolumn = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(DisplayAttribute));
                    if (displaycolumn != null) {
                        name = ((DisplayAttribute)displaycolumn).Name;
                    }
                }
                lstNames[key] = name;
                var stringformat = p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(StringFormatAttribute));
                if (stringformat != null) {
                    lstFormat.Add(key, (StringFormatAttribute)stringformat);
                }
            }

            foreach (var d in lstBaseEntity) {
                var dict = new Dictionary<string, string>();
                foreach (var name in lstNames) {
                    var value = d.GetPropertyValue(name.Key);
                    if (value == null) {
                        dict[name.Value] = string.Empty;
                    } else {
                        if (lstFormat.ContainsKey(name.Key)) {
                            dict[name.Value] = lstFormat[name.Key].Format(value);
                        } else {
                            dict[name.Value] = value.ToString();
                        }
                    }

                }
                datalist.Add(dict);
            }
            return datalist;
        }

        public static HashGroup<TKey, TModel> ToHashGroup<TKey, TModel>(this IEnumerable<BaseEntity> lstBaseEntity, Func<TModel, TKey> func) where TModel : BaseEntity
        {
            if (func == null) {
                return null;
            }
            HashGroup<TKey, TModel> group = new HashGroup<TKey, TModel>();
            foreach (var entity in lstBaseEntity) {
                var key = func.Invoke((TModel)entity);
                group.AddModel(key, (TModel)entity);
            }
            return group;
        }
        private readonly static JsonSerializerSettings s_JsonSetting = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        public static string ToJson(this BaseEntity baseEntity)
        {
            var str = JsonConvert.SerializeObject(baseEntity, s_JsonSetting);
            return str;
        }
        public static string ToJson<T>(this IEnumerable<T> lst) where T : BaseEntity
        {
            var str = JsonConvert.SerializeObject(lst, s_JsonSetting);
            return str;
        }
        public static object FromJson(this BaseEntity baseEntity, string json)
        {
            return JsonConvert.DeserializeObject(json, baseEntity.GetType());
        }

        public static bool Validation(this BaseEntity baseEntity, out Dictionary<string, ICollection<string>> errors)
        {
            errors = new Dictionary<string, ICollection<string>>();
            var result = true;
            var entitytype = baseEntity.GetType();
            foreach (var property in entitytype.GetProperties()) {
                if (property.GetGetMethod() == null) {
                    continue;
                }
                var getmethod = property.GetGetMethod();
                if (getmethod.GetParameters().Length > 0) {
                    continue;
                }
                var attrs = property.GetCustomAttributes(false).Cast<Attribute>().Where(a => a.GetType().GetTypeInfo().IsSubclassOf(typeof(ValidationAttribute)));
                object value = property.GetValue(baseEntity, null);
                foreach (ValidationAttribute attr in attrs) {
                    if (!attr.IsValid(value)) {
                        if (!errors.ContainsKey(property.Name)) {
                            errors.Add(property.Name, new List<string>());
                        }
                        errors[property.Name].Add(attr.FormatErrorMessage(""));
                        result = false;
                    }
                }
            }
            return result;
        }
        public static string Validation(this BaseEntity baseEntity, string propertyname)
        {
            var entitytype = baseEntity.GetType();
            var property = entitytype.GetProperty(propertyname);
            var errmsg = string.Empty;
            if (property == null) {
                return errmsg;
            }
            var attrs = property.GetCustomAttributes(false).Cast<Attribute>().Where(a => a.GetType().GetTypeInfo().IsSubclassOf(typeof(ValidationAttribute)));
            object value = property.GetValue(baseEntity, null);
            foreach (ValidationAttribute attr in attrs) {
                if (!attr.IsValid(value)) {
                    errmsg = attr.FormatErrorMessage("");
                    break;
                }
            }
            return errmsg;
        }
    }
}
