using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChaosCore.Ioc
{
    public static class ServiceProviderIocExtensions
    {
        public static object GetService(this IServiceProvider provider, string key)
        {
            var ioc = provider.GetService(typeof(IIocContext)) as IIocContext;
            if(ioc == null){
                return null;
            }
            var obj = ioc.GetObject(key);
            return obj;
        }

        public static T GetService<T>(this IServiceProvider provider, string key)
        {
            var ioc = provider.GetService(typeof(IIocContext)) as IIocContext;
            if (ioc == null){
                return default(T);
            }
            var obj = ioc.GetObject<T>(key);
            return obj;
        }
    }
}
