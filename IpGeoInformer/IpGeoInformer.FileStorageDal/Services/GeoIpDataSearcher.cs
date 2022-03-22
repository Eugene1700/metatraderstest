using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IpGeoInformer.Domain.Model;
using IpGeoInformer.Domain.Services;
using IpGeoInformer.FileStorageDal.Entities;
using IpGeoInformer.FileStorageDal.Helpers;
using IpGeoInformer.FileStorageDal.Services.Comparers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace IpGeoInformer.FileStorageDal.Services
{
    public class GeoIpDataSearcher : IGeoIpSearcher
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<GeoIpDataSearcher> _logger;
        private byte[] _intervals;
        private byte[] _places;
        private HeaderStruct? _header;
        private byte[] _indexes;

        public GeoIpDataSearcher(IMemoryCache memoryCache, ILogger<GeoIpDataSearcher> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        /// <summary>
        /// IP-интервалы
        /// </summary>
        private byte[] Intervals => _intervals ??= _memoryCache.Get<byte[]>(GeoIpDataBaseDescriptor.IntervalsKey);
        /// <summary>
        /// Локации
        /// </summary>
        private byte[] Locations => _places ??= _memoryCache.Get<byte[]>(GeoIpDataBaseDescriptor.LocationsKey);
        /// <summary>
        /// Заголовок
        /// </summary>
        private HeaderStruct HeaderStruct => _header ??= _memoryCache.Get<HeaderStruct>(GeoIpDataBaseDescriptor.HeaderKey);
        /// <summary>
        /// Индексы
        /// </summary>
        private byte[] Indexes => _indexes ??= _memoryCache.Get<byte[]>(GeoIpDataBaseDescriptor.IndexesKey);
        
        /// <inheritdoc />
        public Location SearchLocationByIp(uint ip)
        {
            IpIntervalStruct ToStruct(int mid) => Intervals.ToStruct<IpIntervalStruct>(mid * GeoIpDataBaseDescriptor.IntervalsSize);
            var result = Intervals.BinarySearch(ip, GeoIpDataBaseDescriptor.IntervalsSize, ToStruct,
                new IpIntervalsComparer());
            if (result == null)
                return null;
            var (interval, _) = result;
            var place = GetLocationByIndex((int) interval.LocationIndex);
            return ToLocation(place);
        }

        /// <inheritdoc />
        public Location[] SearchLocationsByCity(string city)
        {
            var cityComparer = new CityComparer();
            //Поиск проводим по индексу, а сравниваем из локаций
            var searchResult = Indexes.BinarySearch(city, GeoIpDataBaseDescriptor.IndexSize,
                GetLocationByIndex, cityComparer);
            if (searchResult == null)
                return new Location[0];
            var (place, index) = searchResult;
            var res = new List<LocationStruct> {place};
            //нашли какую-то локацию, теперь надо пробежаться вперед и назад и получить полный список для города
            bool CompareAndAddToRes(int inc)
            {
                var s = GetLocationByIndex(inc);
                if (cityComparer.Compare(city, s) != 0) return false;
                res.Add(s);
                return true;
            }

            var i = index - 1;
            while (true)
            {
                if (!CompareAndAddToRes(i--)) break;
            }

            i = index + 1;
            while (true)
            {
                if (!CompareAndAddToRes(i++)) break;
            }

            return res.Select(ToLocation).ToArray();
        }

        /// <summary>
        /// Получить локацию по индексу
        /// </summary>
        /// <param name="index">Индекс без учета размера элемента</param>
        /// <returns>Локация</returns>
        private LocationStruct GetLocationByIndex(int index)
        {
            var locationIndex = Indexes.ToStruct<int>(index * GeoIpDataBaseDescriptor.IndexSize);
            var place = Locations.ToStruct<LocationStruct>(locationIndex);
            return place;
        }
        
        private static Location ToLocation(LocationStruct locationStruct)
        {
            return new Location
            {
                Country = locationStruct.Country,
                City = locationStruct.City,
                Latitude = locationStruct.Latitude,
                Longitude = locationStruct.Longitude,
                Organization = locationStruct.Organization,
                Postal = locationStruct.Postal,
                Region = locationStruct.Region
            };
        }
    }
}