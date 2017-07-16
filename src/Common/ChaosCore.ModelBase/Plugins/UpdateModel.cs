using System;
using System.Collections.Generic;
using System.Text;

namespace ChaosCore.ModelBase.Plugins
{
    public class UpdateModel<T>
    {
        public T ID { get; set; }
        public IEnumerable<ColumnModel> Columns { get; set; }
    }
}
