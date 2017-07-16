using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ChaosCore.CommonLib
{
    public static class ServiceProviderExtensions
    {
        public static bool IsRoot(this IServiceProvider serviceprovider)
        {
            var property = serviceprovider.GetType().GetProperty("Root", BindingFlags.Instance | BindingFlags.NonPublic);
            if(property == null) {
                return false;
            }
            var root = property.GetValue(serviceprovider) as IServiceProvider;
            return root == serviceprovider;
        }
        public static T GetService<T>(this IServiceProvider provider, Action<T> func)
        {
            var target = provider.GetService<T>();
            func.Invoke(target);
            return target;
        }
        public static void UsingService<T>(this IServiceProvider provider, Action<T> func) where T : IDisposable
        {
            using (var target = provider.GetService<T>()){
                func.Invoke(target);
            }
        }
        public static TResult UsingService<T,TResult>(this IServiceProvider provider, Func<T, TResult> func) where T:IDisposable
        {
            using (var target = provider.GetService<T>()){
                var result = func.Invoke(target);
                return result;
            }
        }
    }
}
