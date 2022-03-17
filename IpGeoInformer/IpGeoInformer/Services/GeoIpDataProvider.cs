using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using IpGeoInformer.Helpers;
using IpGeoInformer.Models;
using Microsoft.Extensions.Caching.Memory;

namespace IpGeoInformer.Services
{
    public class GeoIpDataProvider : IGeoIpSearcher
    {
        private readonly IMemoryCache _memoryCache;
        private byte[] _intervals;
        private byte[] _places;
        private Header? _header;
        private byte[] _indexes;
        public GeoIpDataProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        private byte[] Intervals => _intervals ??= _memoryCache.Get<byte[]>(GeoIpConsts.IntervalsKey);
        private byte[] Places => _places ??= _memoryCache.Get<byte[]>(GeoIpConsts.PlacesKey);
        private Header Header => _header ??= _memoryCache.Get<Header>(GeoIpConsts.HeaderKey);
        private byte[] Indexes => _indexes ??= _memoryCache.Get<byte[]>(GeoIpConsts.IndexesKey);

        public PlaceDto SearchPlaceByIp(string ip)
        {
            var intIp = ToUInt(ip);
            var (interval, index) = BinarySearch(Intervals, intIp, new IpIntervalsComparer());
            if (index == null)
                return null;
            var place = GetPlaceByIndex((int) interval.LocationIndex);
            return ToPlaceDto(place);
        }

        private static PlaceDto ToPlaceDto(Place place)
        {
            return new PlaceDto
            {
                Country = place.Country,
                City = place.City,
                Latitude = place.Latitude,
                Longitude = place.Longitude,
                Organization = place.Organization,
                Postal = place.Postal,
                Region = place.Region
            };
        }

        public PlaceDto[] SearchPlacesByCity(string city)
        {
            var (place, index) = BinarySearchByIndex(city);
            if (index == null)
                return new PlaceDto[0];
            var res = new List<Place> {place};

            var i = index.Value - 1;
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
            i = index.Value + 1;
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

        public IpInterval[] GetAllIntervals()
        {
            using var stream = new MemoryStream(Intervals);
            using var binaryReader = new BinaryReader(stream);
            var intervals = new IpInterval[Header.Records];
            for (var i = 0; i < Header.Records; i++)
            {
                intervals[i] = Intervals.ToStruct<IpInterval>(i * GeoIpConsts.IntervalsSize);
            }

            return intervals;
        }

        public Place[] GetAllPlaces()
        {
            using var stream = new MemoryStream(Places);
            using var binaryReader = new BinaryReader(stream);
            var places = new Place[Header.Records];
            for (var i = 0; i < Header.Records; i++)
            {
                places[i] = Places.ToStruct<Place>(i * GeoIpConsts.PlaceSize);
            }

            return places;
        }

        public static IPAddress UInt32ToIpAddress(UInt32 address)
        {
            return new IPAddress(new[]
            {
                (byte) ((address >> 24) & 0xFF),
                (byte) ((address >> 16) & 0xFF),
                (byte) ((address >> 8) & 0xFF),
                (byte) (address & 0xFF)
            });
        }
        
        private Place GetPlaceByIndex(int index)
        {
            var locationIndex = Indexes.ToStruct<int>(index * GeoIpConsts.IndexSize);
            var place = Places.ToStruct<Place>(locationIndex);
            return place;
        }

        private static uint ToUInt(string addr)
        {
            return (uint) IPAddress.NetworkToHostOrder(
                (int) IPAddress.Parse(addr).Address);
        }

        private static (T, int?) BinarySearch<TKey, T>(byte[] inputArray, TKey key,
            IShinyComparer<TKey, T> comparator)
        {
            var size = Marshal.SizeOf<T>();
            var min = 0;
            var max = inputArray.Length / size - 1;
            while (min <= max)
            {
                var mid = (min + max) / 2;
                var structInterval = inputArray.ToStruct<T>(mid * size);
                var compRes = comparator.Compare(key, structInterval);
                if (compRes == 0)
                {
                    return (structInterval, mid);
                }

                if (compRes < 0)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }

            return (default, null);
        }
        
        private (Place, int?) BinarySearchByIndex(string key)
        {
            var min = 0;
            var max = Indexes.Length / GeoIpConsts.IndexSize - 1;
            while (min <= max)
            {
                var mid = (min + max) / 2;
                var place = GetPlaceByIndex(mid);
                var compRes = key.CompareTo(place.City);
                if (compRes == 0)
                {
                    return (place, mid);
                }

                if (compRes < 0)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }

            return (default, null);
        }
    }
}