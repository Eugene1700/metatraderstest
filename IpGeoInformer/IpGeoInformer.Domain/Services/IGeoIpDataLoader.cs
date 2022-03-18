namespace IpGeoInformer.Domain.Services
{
    public interface IGeoIpDataLoader
    {
        void Load(string filePath);
    }
}