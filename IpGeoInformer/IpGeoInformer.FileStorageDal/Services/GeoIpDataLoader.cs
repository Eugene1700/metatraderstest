using System.IO;
using IpGeoInformer.Domain.Services;
using IpGeoInformer.FileStorageDal.Entities;
using IpGeoInformer.FileStorageDal.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace IpGeoInformer.FileStorageDal.Services
{
    public class GeoIpDataLoader : IGeoIpDataLoader
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
            
            var headerBytes = binaryReader.ReadBytes(GeoIpDataBaseDescriptor.HeaderSize);
            var header = headerBytes.ToStruct<HeaderStruct>(0);
            
            var records = header.Records;
            var intervals = binaryReader.ReadBytes(GeoIpDataBaseDescriptor.IntervalsSize * records);
            var places = binaryReader.ReadBytes(GeoIpDataBaseDescriptor.LocationSize * records);
            var indexes = binaryReader.ReadBytes(GeoIpDataBaseDescriptor.IndexSize * records);
            
            _memoryCache.Set(GeoIpDataBaseDescriptor.HeaderKey, header);
            _memoryCache.Set(GeoIpDataBaseDescriptor.IntervalsKey, intervals);
            _memoryCache.Set(GeoIpDataBaseDescriptor.LocationsKey, places);
            _memoryCache.Set(GeoIpDataBaseDescriptor.IndexesKey, indexes);
        }
    }
}