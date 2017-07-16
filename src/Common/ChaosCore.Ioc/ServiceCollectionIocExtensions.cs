using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ChaosCore.Ioc.Model;
using System.Linq.Expressions;

namespace ChaosCore.Ioc
{
    public static class ServiceCollectionIocExtensions
    {
        public const string STR_SECTION_SERVICECOLLECTION = "ServiceCollection";
        public const string STR_SECTION_SERVICECOLLECTION_EXTENSIONS = "ServiceCollectionExtensions";
        public static IServiceCollection AddIocConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            System.Diagnostics.Debug.WriteLine("AddIocConfiguration START.");
            var section = configuration.GetSection(STR_SECTION_SERVICECOLLECTION);
            #region ServiceCollection
            var models = section.Get<ServiceModel[]>();
            System.Diagnostics.Debug.WriteLineIf(models == null, "models is null");
            foreach (var model in models) {
                var serviceType = Type.GetType(model.ServiceType);
                var implementationType = Type.GetType(model.ImplementationType);
                if (implementationType == null) {
                    var substring = model.ImplementationType.Split(',');
                    var assembly = Assembly.Load(new AssemblyName(substring[1]));
                    implementationType = assembly.GetType(substring[0]);
                    implementationType = AssemblyExtension.GetType(model.ImplementationType);
                }
                var lifetime = model.GetServiceLifetime();
                if (model.Properties == null || !model.Properties.Any()) {
                    services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
                } else {
                    var para = Expression.Parameter(typeof(IServiceProvider), "_");

                    var exprNew = Expression.New(implementationType);
                    var memberassign = model.Properties.Select(p => Expression.Bind(implementationType.GetTypeInfo().GetProperty(p.PropertyName), Expression.Constant(p.Value))).ToArray();
                    var expr = Expression.MemberInit(exprNew, memberassign);
                    var labmbda = Expression.Lambda<Func<IServiceProvider, object>>(expr, para).Compile();
                    services.Add(new ServiceDescriptor(serviceType, labmbda, lifetime));
                }
            }
            #endregion

            #region ServiceCollectionExtensions
            section = configuration.GetSection(STR_SECTION_SERVICECOLLECTION_EXTENSIONS);
            var exmodel = section.Get<ExtensionModel[]>();
            foreach (var model in exmodel) {
                var method = model.GetMethod();
                if (method != null) {
                    var parainfos = method.GetParameters();
                    var para = new object[parainfos.Length];
                    byte i = 0;
                    foreach (var info in parainfos) {
                        if (info.ParameterType == typeof(IServiceCollection) || info.ParameterType.GetTypeInfo().IsSubclassOf(typeof(IServiceCollection))) {
                            para[i] = services;
                        } else if (info.ParameterType == typeof(IConfiguration) || info.ParameterType.GetTypeInfo().IsSubclassOf(typeof(IConfiguration))) {
                            para[i] = configuration;
                        }
                        i++;
                    }
                    services = method.Invoke(null, para) as IServiceCollection;
                }
            }
            #endregion
            return services;
        }
        public static IServiceCollection AddJsonIocConfiguration(this IServiceCollection services, string filename)
        {
            var file = new System.IO.FileInfo(filename);
            var fullname = file.FullName;
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(fullname, optional: true, reloadOnChange: false).Build();

            return AddIocConfiguration(services, configuration);
        }
    }
}
