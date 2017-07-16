using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.Ioc
{
    public interface IIocContext
    {
        void SetConfiguration(IConfiguration configuration);
        object GetObject(string key);
        T GetObject<T>(string key);
    }
}
