using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using IpGeoInformer.Domain;
using IpGeoInformer.Domain.Model;
using IpGeoInformer.Domain.Services;
using IpGeoInformer.FileStorageDal.Entities;
using IpGeoInformer.FileStorageDal.Helpers;
using IpGeoInformer.FileStorageDal.Services.Comparers;
using IpGeoInformer.Services;
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

        private byte[] Intervals => _intervals ??= _memoryCache.Get<byte[]>(GeoIpDataBaseDescriptor.IntervalsKey);
        private byte[] Places => _places ??= _memoryCache.Get<byte[]>(GeoIpDataBaseDescriptor.PlacesKey);
        private HeaderStruct HeaderStruct => _header ??= _memoryCache.Get<HeaderStruct>(GeoIpDataBaseDescriptor.HeaderKey);
        private byte[] Indexes => _indexes ??= _memoryCache.Get<byte[]>(GeoIpDataBaseDescriptor.IndexesKey);

        public Place SearchPlaceByIp(uint ip)
        {
            // var intIp = StrIpToUInt(ip);
            IpIntervalStruct ToStruct(int mid) => Intervals.ToStruct<IpIntervalStruct>(mid * GeoIpDataBaseDescriptor.IntervalsSize);
            var result = Intervals.BinarySearch(ip, GeoIpDataBaseDescriptor.IntervalsSize, ToStruct,
                new IpIntervalsComparer());
            if (result == null)
                return null;
            var (interval, _) = result;
            var place = GetPlaceByIndex((int) interval.LocationIndex);
            return ToPlaceDto(place);
        }

        private static Place ToPlaceDto(PlaceStruct placeStruct)
        {
            return new Place
            {
                Country = placeStruct.Country,
                City = placeStruct.City,
                Latitude = placeStruct.Latitude,
                Longitude = placeStruct.Longitude,
                Organization = placeStruct.Organization,
                Postal = placeStruct.Postal,
                Region = placeStruct.Region
            };
        }

        public Place[] SearchPlacesByCity(string city)
        {
            PlaceStruct ToStruct(int mid) => GetPlaceByIndex(mid);
            var searchResult = Indexes.BinarySearch(city, GeoIpDataBaseDescriptor.IndexSize, 
                ToStruct, new CityComparer());
            if (searchResult == null)
                return new Place[0];
            var (place, index) = searchResult;
            var res = new List<PlaceStruct> {place};

            var i = index - 1;
            while (true)
            {
                var s = GetPlaceByIndex(i);
                if (s.City == city)
                {
                    res.Add(s);
                }
                else
                {
                    break;
                }

                --i;
            }

            i = index + 1;
            while (true)
            {
                var s = GetPlaceByIndex(i);
                if (s.City == city)
                {
                    res.Add(s);
                }
                else
                {
                    break;
                }

                ++i;
            }

            return res.Select(ToPlaceDto).ToArray();
        }

        public IpIntervalStruct[] GetAllIntervals()
        {
            using var stream = new MemoryStream(Intervals);
            using var binaryReader = new BinaryReader(stream);
            var intervals = new IpIntervalStruct[HeaderStruct.Records];
            for (var i = 0; i < HeaderStruct.Records; i++)
            {
                intervals[i] = Intervals.ToStruct<IpIntervalStruct>(i * GeoIpDataBaseDescriptor.IntervalsSize);
            }

            return intervals;
        }

        public PlaceStruct[] GetAllPlaces()
        {
            using var stream = new MemoryStream(Places);
            using var binaryReader = new BinaryReader(stream);
            var places = new PlaceStruct[HeaderStruct.Records];
            for (var i = 0; i < HeaderStruct.Records; i++)
            {
                places[i] = Places.ToStruct<PlaceStruct>(i * GeoIpDataBaseDescriptor.PlaceSize);
            }

            return places;
        }
        
        private PlaceStruct GetPlaceByIndex(int index)
        {
            var locationIndex = Indexes.ToStruct<int>(index * GeoIpDataBaseDescriptor.IndexSize);
            var place = Places.ToStruct<PlaceStruct>(locationIndex);
            return place;
        }
    }
}