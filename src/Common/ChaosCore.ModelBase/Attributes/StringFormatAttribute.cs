using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase.Attributes
{
    public class StringFormatAttribute: Attribute
    {
        public string FormatString { get; set; }
        public StringFormatAttribute(string format)
        {
            FormatString = format;
        }

        public string Format(object obj)
        {
            return string.Format("{0:" + FormatString + "}", obj);
        }
    }
}
