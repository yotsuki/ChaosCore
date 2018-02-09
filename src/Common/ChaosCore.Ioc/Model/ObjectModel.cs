using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.Ioc
{
    public class ObjectModel
    {
        public string Key { get; set; }
        public string TypeName { get; set; }
        public Type InstanceType { get; set; }
        public Dictionary<string, PropertyModel> Properties { get; set; }
    }
}
