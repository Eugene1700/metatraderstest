namespace IpGeoInformer
{
    public interface IGeoIpSearcher
    {
        Place SearchPlaceByIp(string ip);
        Place[] SearchPlacesByCity(string city);
    }
}