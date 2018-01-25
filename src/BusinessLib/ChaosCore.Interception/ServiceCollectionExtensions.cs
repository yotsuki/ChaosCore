using Microsoft.Extensions.DependencyInjection;

namespace ChaosCore.Interception
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInterceptionService(this IServiceCollection services)
        {
            services.AddSingleton(services);
            services.AddSingleton<ServiceProvider2>(sp => (ServiceProvider2)sp.GetService<IServiceCollection>().BuildInterceptableServiceProvider());
            return services;
        }
    }
}
