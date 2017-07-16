using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ChaosCore.Ioc
{
    public static class AssemblyExtension
    {
        private static Dictionary<string, Type> s_mapTypes = new Dictionary<string, Type>();
        private static Dictionary<string, Assembly> s_mapAssemblys = new Dictionary<string, Assembly>();
        public static Type GetType(string fulltypename, string dllpath = null)
        {
            Type type = null;
            if (!s_mapTypes.TryGetValue(fulltypename, out type)) {
                type = Type.GetType(fulltypename);
                if (type == null) {
                    var cell = fulltypename.Split(',');
                    if (cell.Length == 1) {  // typename only
                        throw new Exception("not full type name."); ;
                    } else if (cell.Length >= 2) {
                        var typename = cell[0].Trim();
                        var assemblyname = string.Join(", ", cell.Skip(1));
                        var assembly = GetAssembly(assemblyname, dllpath);
                        if (assembly != null) {
                            type = assembly.GetType(typename);
                            if (type != null) {
                                s_mapTypes.Add(fulltypename, type);
                            }
                        }
                    } else {
                        throw new Exception("Type name is error.");
                    }
                }
            }
            return type;
        }

        private static Assembly GetAssembly(string assemblyname, string dllpath = null)
        {
            Assembly assembly = null;
            if (!s_mapAssemblys.TryGetValue(assemblyname, out assembly)) {
                try
                {
                    assembly = Assembly.Load(new AssemblyName(assemblyname));
                }catch(Exception){

                }
                if (assembly == null){
                    var cell = assemblyname.Split(',');
                    var name = cell[0].Trim();
                    var fullpath = string.IsNullOrEmpty(dllpath) ? Path.Combine(AppContext.BaseDirectory, name + ".dll") : dllpath;
                    assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(new FileInfo(fullpath).FullName);
                }
                if (assembly != null){
                    s_mapAssemblys.Add(assemblyname, assembly);
                }
            }
            return assembly;
        }
    }
}
