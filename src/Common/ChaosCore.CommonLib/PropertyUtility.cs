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
        public static void SetPropertyValue(this object obj,string propertyName, object value)
        {
            if(obj == null) {
                return;
            }
            var type = obj.GetType();
            var pi = type.GetProperty(propertyName);
            if(pi == null) {
                return;
            }
            if (pi.SetMethod == null && !pi.SetMethod.IsPublic) {
                return;
            }
            if(value == null) {
                pi.SetValue(obj, value);
            }
            var valuetype = value.GetType();
            if (!valuetype.GetTypeInfo().IsSubclassOf(pi.PropertyType)) {
                throw new Exception("");
            }
            pi.SetValue(obj, value);
        }
    }
}
