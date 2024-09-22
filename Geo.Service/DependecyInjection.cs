using Microsoft.Extensions.DependencyInjection;

namespace Geo.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            return services;
        }
    };
}
