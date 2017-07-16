using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChaosCore.Ioc.Model
{
    public class ServiceModel
    {
        internal static readonly string[] LifetimeStrings = new string[] { "transient", "scoped", "singleton" };
        public string ServiceType { get; set; }
        public string ImplementationType { get; set; }
        private string _lifetime = "transient";
        public string Lifetime { get {
                return _lifetime;
            } set {
                var lower = value.ToLower();
                if (LifetimeStrings.Contains(lower)) {
                    _lifetime = lower;
                }
            }
        }

        public IEnumerable<PropertyModel> Properties { get; set; }
        public IEnumerable<ConstructorModel> Constructors { get; set; }
    }
    public static class ServiceModelExtension
    {
        public static ServiceLifetime GetServiceLifetime(this ServiceModel model)
        {
            return (ServiceLifetime)Enum.Parse(typeof(ServiceLifetime), model.Lifetime, true);
        }
    }
}
