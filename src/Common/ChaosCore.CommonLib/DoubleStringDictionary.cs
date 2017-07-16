using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.CommonLib
{
    public class DoubleStringDictionary : Dictionary<string, string>
    {
        public bool AddItemIfNotEmpty(string key, string value)
        {
            if (!string.IsNullOrEmpty(value)) {
                this.Add(key, value);
                return true;
            } else {
                return false;
            }
        }
        public new string this[string key]
        {
            get { return ContainsKey(key) ? base[key] : string.Empty; }
            set { base[key] = value; }
        }

        public string GetValue(params string[] keys)
        {
            string value = null;
            foreach(var key in keys) {
                if (ContainsKey(key)) {
                    value = base[key];
                    break;
                }
            }
            return value;
        }
    }
}
