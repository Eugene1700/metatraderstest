using IpGeoInformer.Domain.Model;

namespace IpGeoInformer.Domain.Services
{
    public interface IGeoIpSearcher
    {
        Place SearchPlaceByIp(uint ip);
        Place[] SearchPlacesByCity(string city);
    }
}