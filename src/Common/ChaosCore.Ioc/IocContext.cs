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
            if (_configuration == null || _objectsSection == null) {
                return null;
            }
            var section = _objectsSection.GetSection(key);
            if (section == null) {
                return null;
            }
            var obj = section.Get<ObjectModel>();
            return GetObject(obj);
        }

        private object GetObject(ObjectModel obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.TypeName)) {
                return null;
            }
            var type = AssemblyExtension.GetType(obj.TypeName);
            var result = Activator.CreateInstance(type);
            if (obj.Properties != null && obj.Properties.Any()) {
                foreach (var pro in obj.Properties) {
                    object value = null;
                    if (!string.IsNullOrEmpty(pro.Value.Ref)) {
                        value = GetObject(pro.Value.Ref);
                    }else if(pro.Value.Value != null) {
                        value = GetObject(pro.Value.Value);
                    }
                    SetPropertyValue(result, pro.Key, value);
                }
            }
            return result;
        }

        public T GetObject<T>(string key)
            => (T)GetObject(key);

        private bool SetPropertyValue(object obj, string propertyName, object value)
        {
            if (obj == null) {
                return false;
            }
            var type = obj.GetType();
            var pi = type.GetTypeInfo().GetProperty(propertyName);
            if (pi == null) {
                return false;
            }
            if (pi.SetMethod == null || !pi.SetMethod.IsPublic) {
                return false;
            }
            if (value == null) {
                pi.SetValue(obj, value);
                return true;
            }
            var valuetype = value.GetType();
            if (valuetype != pi.PropertyType && !pi.PropertyType.GetTypeInfo().IsAssignableFrom(valuetype)) {
                return false;
            }
            pi.SetValue(obj, value);
            return true;
        }
    }
}
