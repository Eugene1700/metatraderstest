using IpGeoInformer.Domain.Services;
using IpGeoInformer.FileStorageDal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IpGeoInformer.FileStorageDal
{
    public static class CompositionRoot
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGeoIpDataLoader, GeoIpDataLoader>();
            services.AddScoped<IGeoIpSearcher, GeoIpDataSearcher>();
        }
    }
}