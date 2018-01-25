using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChaosCore.CommonLib.Configuration
{
    public static class ConfigurationStorage
    {
        private static IDictionary<string, IConfiguration> m_map = new Dictionary<string, IConfiguration>();
        public static IConfiguration Default {
            get {
                return m_map.TryGetValue("Default", out var config) ? config : null;
            }
        }
        public static IConfiguration Get(string key)
        {
            return m_map.TryGetValue(key, out var config) ? config : null;
        }
        public static IConfiguration AddToStorage(this IConfiguration config,string key = "Default")
        {
            m_map[key] = config;
            return config;
        }
    }
}
