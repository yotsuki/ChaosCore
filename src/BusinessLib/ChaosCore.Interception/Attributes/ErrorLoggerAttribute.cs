using Dora.Interception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.Interception.Attributes
{
    public class ErrorLoggerAttribute : InterceptorAttribute
    {
        private string _category;
    
        public ErrorLoggerAttribute(string category)
        {
            _category = category;
        }
        public override void Use(IInterceptorChainBuilder builder)
        {
            builder.Use<ErrorLogger>(this.Order, _category);
        }
}
}
