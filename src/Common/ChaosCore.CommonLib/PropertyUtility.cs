using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChaosCore.Common.Utilites
{
    public static class PropertyUtility
    {
        public static bool SetPropertyValue(this object obj,string propertyName, object value)
        {
            if(obj == null) {
                return false;
            }
            var type = obj.GetType();
            var pi = type.GetProperty(propertyName);
            if(pi == null) {
                return false;
            }
            if (pi.SetMethod == null || !pi.SetMethod.IsPublic) {
                return false;
            }
            if(value == null) {
                pi.SetValue(obj, value);
                return true;
            }
            var valuetype = value.GetType();
            if (valuetype!=pi.PropertyType && !pi.PropertyType.GetTypeInfo().IsAssignableFrom(valuetype)) {
                return false;
            }
            pi.SetValue(obj, value);
            return true;
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if(obj == null) {
                throw new Exception("object is null.");
            }
            var type = obj.GetType();
            var pi = type.GetProperty(propertyName);
            if (pi == null) {
                throw new Exception($"property '{propertyName}' not found.");
            }
            if (pi.GetMethod == null || !pi.GetMethod.IsPublic) {
                throw new Exception($"get method is null or not public by property '{propertyName}'.");
            }
            return pi.GetValue(obj);
        }
        public static bool TryGetPropertyValue(this object obj, string propertyName, out object value)
        {
            try {
                value = GetPropertyValue(obj, propertyName);
                return true;
            } catch {
                value = null;
                return false;
            }
        }
    }
}
