using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ChaosCore.Ioc
{
    public class IocContext: IIocContext
    {
        public const string STR_SECTION_OBJECTS = "objects";
        public const string STR_CONFIG_FILENAME = "ioc.json";
        private IConfiguration _configuration = null;
        private IConfigurationSection _objectsSection = null;
        
        public IocContext()
        {

        }
        public IocContext(string configname)
        {
            var file = new System.IO.FileInfo(configname);
            var fullname = file.FullName;
            var builder = new ConfigurationBuilder()
                .AddJsonFile(fullname, optional: true, reloadOnChange: false);
            SetConfiguration(builder.Build());
        }

        public IocContext(IConfiguration configuration)
        {
            SetConfiguration(configuration);
        }

        public void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            _objectsSection = _configuration.GetSection(STR_SECTION_OBJECTS);
            if (_objectsSection == null) {
                throw new Exception($"Not found section '{STR_SECTION_OBJECTS}'");
            }
        }

        public object GetObject(string key)
        {
            if(_configuration == null || _objectsSection == null) {
                return null;
            }
            var section = _objectsSection.GetSection(key);
            if(section == null) {
                return null;
            }
            var obj = section.Get<ObjectModel>();
            if(obj == null || string.IsNullOrEmpty(obj.TypeName)) {
                return null;
            }
            var type = AssemblyExtension.GetType(obj.TypeName);
            return Activator.CreateInstance(type);
        }
        
        public T GetObject<T>(string key)
            => (T)GetObject(key);
    }
}
