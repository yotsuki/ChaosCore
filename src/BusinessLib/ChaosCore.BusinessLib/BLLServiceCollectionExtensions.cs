using ChaosCore.CommonLib;
using Microsoft.Extensions.DependencyInjection;

namespace ChaosCore.BusinessLib
{
    public static class BLLServiceCollectionExtensions
    {
        public static IServiceCollection AddBLL(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICommonResource, DefaultCommonResource>()
                .AddScoped<IBusinessSession, BusinessSession>()
                .AddScoped<IUserBLL, UserBLL>()
                .AddScoped<IConfigBLL, ConfigBLL>()
                .AddScoped<IAuthorizationTokenBLL, AuthorizationTokenBLL>()
                .AddScoped<IRoleBLL, RoleBLL>();
        }
    }
}
