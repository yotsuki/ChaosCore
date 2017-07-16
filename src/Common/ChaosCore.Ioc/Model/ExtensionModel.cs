using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ChaosCore.Ioc.Model
{
    public class ExtensionModel
    {
        public string Type { get; set; }
        public string Method { get; set; }
    }

    public static class ExtensionModelExtension
    {
        public static MethodInfo GetMethod(this ExtensionModel extensionModel)
        {
            var type = AssemblyExtension.GetType(extensionModel.Type);
            if(type == null) {
                return null;
            }
            return type.GetTypeInfo().GetMethod(extensionModel.Method);
        }
    }
}
