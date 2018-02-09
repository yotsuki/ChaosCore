using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChaosCore.CommonLib.Configuration
{
    public static class ConfigurationExtension
    {
        public static IConfigurationBuilder UseFiles(this IConfigurationBuilder builder, string configfiles, string environmentKey = null)
            => UseFiles(builder, configfiles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries), environmentKey);
        
        public static IConfigurationBuilder UseFiles(this IConfigurationBuilder builder, string[] files, string environmentKey = null)
        {
            foreach (var filename in files) {
                
                var fi = new FileInfo(filename.ToLower());
                builder.AddFile(filename);
                

                if (!string.IsNullOrWhiteSpace(environmentKey)) {
                    var envfilename = Path.Combine(fi.DirectoryName, $"{fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length)}.{environmentKey}{fi.Extension}");
                    builder.AddFile(envfilename);
                }
            }
            return builder;
        }
        public static IConfigurationBuilder AddFile(this IConfigurationBuilder builder, string filename)
            => AddFile(builder, new FileInfo(filename));

        public static IConfigurationBuilder AddFile(this IConfigurationBuilder builder, FileInfo file)
        {
            switch (file.Extension) {
                case ".json": {
                        builder.AddJsonFile(file.FullName, optional: false, reloadOnChange: true);
                    }
                    break;
                case ".xml": {
                        builder.AddXmlFile(file.FullName, optional: false, reloadOnChange: true);
                    }
                    break;
                case ".ini": {
                        builder.AddIniFile(file.FullName);
                    }
                    break;
                default:
                    throw new FormatException("unknow file format");
            }
            return builder;
        }
    }
}
