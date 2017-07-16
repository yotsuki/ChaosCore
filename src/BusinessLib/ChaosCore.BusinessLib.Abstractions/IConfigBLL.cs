using ChaosCore.ModelBase;
using System.Collections.Generic;

namespace ChaosCore.BusinessLib
{
    public interface IConfigBLL: IBaseBLL<SysConfig>
    {
        string GetValue(string section, string name);

        Dictionary<string, string> GetValues(string setcion);

        Dictionary<string, string> GetAllValues();

        Dictionary<string, Dictionary<string, string>> GetMapValues();

        Result SetValue(string section, string name, string value);

        Result RemoveName(string section, string name);
        Result RemoveSection(string section);

        Result AddConfig(SysConfig config);
    }
}
