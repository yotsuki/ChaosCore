using ChaosCore.BusinessLib;
using ChaosCore.CommonLib;
using ChaosCore.Ioc;
using ChaosCore.ModelBase;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Globalization;
using System.Resources;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace TestCmd
{
    public class HostingEnvironment : IHostingEnvironment
    {
        public string EnvironmentName { get ; set; }
        public string ApplicationName { get ; set ; }
        public string WebRootPath { get ; set ; }
        public IFileProvider WebRootFileProvider { get ; set ; }
        public string ContentRootPath { get ; set ; }
        public IFileProvider ContentRootFileProvider { get ; set ; }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            //var type = AssemblyExtension.GetType("ChaosCore.BusinessLib.RoleBLL, ChaosCore.BusinessLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            var sc = new ServiceCollection();
            sc.AddInterception()
              .AddLogging()
              .AddOptions()
              .AddSingleton<IHostingEnvironment, HostingEnvironment>()
              .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))
              .AddSingleton(typeof(IIocContext), _ => new IocContext(Path.Combine(AppContext.BaseDirectory, "ioc.json")))
              .AddLocalization(o=>o.ResourcesPath="Resources")
              .ToInterceptable();
            var serviceProvider = sc.BuildServiceProvider();
            var loggerfactory = serviceProvider.GetService<ILoggerFactory>();
            loggerfactory.AddConsole();

            using (var bll = serviceProvider.GetService<IUserBLL>(u=>u.UserID = 123)) {
                
                var user = bll.GetUserFromId(1);
                //if(user == null) {
                //    user = bll.SignIn(new User() { Name = "yotsuki",Roles = new List<UserRole>() });
                //}else {
                //    bll.DeleteUser(user.Value.ID);
                //}
            }
        }
    }
}
