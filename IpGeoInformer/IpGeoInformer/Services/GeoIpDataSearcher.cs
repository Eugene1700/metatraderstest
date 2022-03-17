using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using IpGeoInformer.Domain;
using IpGeoInformer.Helpers;
using IpGeoInformer.Services.Comparers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace IpGeoInformer.Services
{
    public class GeoIpDataSearcher : IGeoIpSearcher
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<GeoIpDataSearcher> _logger;
        private byte[] _intervals;
        private byte[] _places;
        private Header? _header;
        private byte[] _indexes;

        public GeoIpDataSearcher(IMemoryCache memoryCache, ILogger<GeoIpDataSearcher> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        private byte[] Intervals => _intervals ??= _memoryCache.Get<byte[]>(GeoIpConsts.IntervalsKey);
        private byte[] Places => _places ??= _memoryCache.Get<byte[]>(GeoIpConsts.PlacesKey);
        private Header Header => _header ??= _memoryCache.Get<Header>(GeoIpConsts.HeaderKey);
        private byte[] Indexes => _indexes ??= _memoryCache.Get<byte[]>(GeoIpConsts.IndexesKey);

        public PlaceDto SearchPlaceByIp(string ip)
        {
            var intIp = StrIpToUInt(ip);
            IpInterval ToStruct(int mid) => Intervals.ToStruct<IpInterval>(mid * GeoIpConsts.IntervalsSize);
            var result = Intervals.BinarySearch(intIp, GeoIpConsts.IntervalsSize, ToStruct,
                new IpIntervalsComparer());
            if (result == null)
                return null;
            var (interval, _) = result;
            //todo удалить
            // _logger.LogInformation(
            //     $"IpFrom = [{UInt32ToIpAddress(interval.IpFrom)}] IpTo = [{UInt32ToIpAddress(interval.IpTo)}]");
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
            Place ToStruct(int mid) => GetPlaceByIndex(mid);
            var searchResult = Indexes.BinarySearch(city, GeoIpConsts.IndexSize, 
                ToStruct, new CityComparer());
            if (searchResult == null)
                return new PlaceDto[0];
            var (place, index) = searchResult;
            var res = new List<Place> {place};

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
        
        private Place GetPlaceByIndex(int index)
        {
            var locationIndex = Indexes.ToStruct<int>(index * GeoIpConsts.IndexSize);
            var place = Places.ToStruct<Place>(locationIndex);
            return place;
        }

        private static uint StrIpToUInt(string ipAddress)
        {
            return (uint) IPAddress.NetworkToHostOrder(
                (int) BitConverter.ToUInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0));
        }
    }
}