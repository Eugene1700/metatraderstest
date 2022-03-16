using IpGeoInformer.Models;

namespace IpGeoInformer.Services
{
    public interface IGeoIpSearcher
    {
        PlaceDto SearchPlaceByIp(string ip);
        PlaceDto[] SearchPlacesByCity(string city);
    }
}