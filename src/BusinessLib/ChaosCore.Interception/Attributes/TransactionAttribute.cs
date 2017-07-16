using Dora.Interception;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChaosCore.Interception.Attributes
{
    public class TransactionAttribute : InterceptorAttribute
    {
        public TransactionAttribute()
        {
        }

        public override void Use(IInterceptorChainBuilder builder)
        {
            builder.Use<Transaction>(this.Order);
        }
    }
}
