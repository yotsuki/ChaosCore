using Dora.Interception;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChaosCore.Interception
{
    public class Transaction
    {
        private InterceptDelegate _next;
        public Transaction(InterceptDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(InvocationContext context)
        {
            try {
                await _next(context);
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
