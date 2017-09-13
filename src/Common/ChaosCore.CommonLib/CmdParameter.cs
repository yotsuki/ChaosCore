using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.CommonLib
{
    public enum ParameterType
    {
        Single,
        Array,
    }
    public class ParameterModel
    {
        public ParameterType Type { get; set; }
        public bool IsSet { get; set; }
        internal string _value = string.Empty;
        internal List<string> _lstValue = new List<string>();
        public string Value {
            get {
                if(Type == ParameterType.Single) {
                    return _value;
                } else {
                    return _lstValue.FirstOrDefault();
                }
            }
        }

        public string Key { get; set; }
    }
    public class CmdParameter
    {
        public CmdParameter()
        {

        }
        public CmdParameter(string[] paras)
        {
            Load(paras);
        }
        public string DefaultParameter { get; set; }
        private readonly Dictionary<string, ParameterModel> m_Dict = new Dictionary<string, ParameterModel>();
        private HashSet<string> Params = new HashSet<string>();
        public void Load(string[] paras)
        {
            string key = "";
            //ParameterModel model = null;
            foreach (var p in paras) {
                if(p[0]== '-') {
                    Params.Add(p);
                    key = p;
                } else {
                    if(key == null) {
                        DefaultParameter = p;
                    } else {
                        ParameterModel model = null;
                        if (!m_Dict.ContainsKey(key)) {
                            model = new ParameterModel() { Key = key,IsSet = true };
                            m_Dict.Add(key, model);
                        } else {
                            model = m_Dict[key];
                        }
                        model._lstValue.Add(p);
                        if(model._value == string.Empty) {
                            model._value = p;
                        }
                    }
                }
            }
        }

        public string this[string key] {
            get {
                if (m_Dict.ContainsKey(key)) {
                    return m_Dict[key].Value;
                } else {
                    return string.Empty;
                }
            }
        }

        public bool IsSet(string key)
        {
            if (Params.Contains(key)) {
                return true;
            } else {
                return false;
            }
        }


    }
}
