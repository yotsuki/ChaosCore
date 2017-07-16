using Dora.Interception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ChaosCore.Interception
{
    public class ErrorLogger
    {
        private InterceptDelegate _next;
        private string _category;
        public ErrorLogger(InterceptDelegate next, string category)
        {
            _next = next;
            _category = category;
        }
      
        public async Task InvokeAsync(InvocationContext context, ILoggerFactory loggerFactory)
        {
            try{
                await _next(context);
            }catch (Exception ex){
                loggerFactory.CreateLogger(_category).LogError(ex.Message);
                throw;
            }
        }
    }
}
