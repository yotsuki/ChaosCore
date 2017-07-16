using ChaosCore.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChaosCore.BusinessLib
{
    public class ConfigBLL : BaseBLL<SysConfig>, IConfigBLL
    {
        public ConfigBLL() { }
        public ConfigBLL(IServiceProvider provider) : base(provider){ }
        public Result AddConfig(SysConfig config)
        {
            var result = new Result();
            if (BllRepository.GetQuery(c => c.Section == config.Section && c.Name == config.Name).Any()) {
                result.Code = -1;
                result.Message = "重复";
                return result;
            }
            BllRepository.Add(config);
            return SaveChanges();
        }

        public Dictionary<string, string> GetAllValues()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var array = BllRepository.GetQuery().Select(c => new { c.Section, c.Name, c.Value }).ToArray();
            foreach(var model in array) {
                result.Add($"{model.Section}.{model.Name}", model.Value);
            }
            return result;
        }

        public Dictionary<string, Dictionary<string, string>> GetMapValues()
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            var array = BllRepository.GetQuery().Select(c => new { c.Section, c.Name, c.Value }).OrderBy(c=>c.Section).ToArray();
            foreach (var model in array) {
                if (!result.ContainsKey(model.Section)) {
                    result.Add(model.Section, new Dictionary<string, string>());
                }
                result[model.Section][model.Name] = model.Value;
            }
            return result;
        }

        public string GetValue(string section, string name)
        {
            var entity = BllRepository.GetQuery(c=>c.Section == section && c.Name == name).FirstOrDefault();
            if(entity == null) {
                return null;
            }else {
                return entity.Value??string.Empty;
            }
        }

        public Dictionary<string, string> GetValues(string section)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var array = BllRepository.GetQuery(c=>c.Section == section).Select(c => new {  c.Name, c.Value }).ToArray();
            foreach (var model in array) {
                result.Add(model.Name, model.Value);
            }
            return result;
        }

        public Result RemoveName(string section, string name)
        {
            var entity = BllRepository.GetQuery(c => c.Section == section && c.Name == name).FirstOrDefault();
            if (entity == null) {
                return false;
            } else {
                BllRepository.Delete(entity);
                return SaveChanges();
            }
        }

        public Result RemoveSection(string section)
        {
            var array = BllRepository.GetQuery(c => c.Section == section ).ToArray();
            foreach (var entity in array) {
                BllRepository.Delete(entity);
            }
            return SaveChanges();
        }

        public Result SetValue(string section, string name, string value)
        {
            var entity = BllRepository.GetQuery(c => c.Section == section && c.Name == name).FirstOrDefault();
            if (entity == null) {
                entity = new SysConfig() { Section = section, Name = name };
                BllRepository.Add(entity);
            } else {
                entity.Value = value;
            }
            return SaveChanges();
        }

     }
}
