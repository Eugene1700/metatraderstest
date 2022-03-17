using System.IO;
using IpGeoInformer.Domain;
using IpGeoInformer.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace IpGeoInformer.Services
{
    public class GeoIpDataLoader
    {
        private readonly IMemoryCache _memoryCache;

        public GeoIpDataLoader(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        
        public void Load(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            using var stream = new MemoryStream(bytes);
            using var binaryReader = new BinaryReader(stream);
            
            var headerBytes = binaryReader.ReadBytes(GeoIpConsts.HeaderSize);
            var header = headerBytes.ToStruct<Header>(0);
            
            var records = header.Records;
            var intervals = binaryReader.ReadBytes(GeoIpConsts.IntervalsSize * records);
            var places = binaryReader.ReadBytes(GeoIpConsts.PlaceSize * records);
            var indexes = binaryReader.ReadBytes(GeoIpConsts.IndexSize * records);
            
            _memoryCache.Set(GeoIpConsts.HeaderKey, header);
            _memoryCache.Set(GeoIpConsts.IntervalsKey, intervals);
            _memoryCache.Set(GeoIpConsts.PlacesKey, places);
            _memoryCache.Set(GeoIpConsts.IndexesKey, indexes);
        }
    }
}