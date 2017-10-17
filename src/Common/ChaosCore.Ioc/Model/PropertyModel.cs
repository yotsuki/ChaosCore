using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.Ioc
{
    public class PropertyModel
    {
        public string PropertyName { get; set; }
        public ObjectModel Value { get; set; }
        public string Ref { get; set; }
    }
}
