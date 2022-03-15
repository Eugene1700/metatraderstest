using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace IpGeoInformer
{
    public class DataBase
    {
        private readonly Header _header;
        private readonly byte[] _intervals;
        private readonly byte[] _places;
        private readonly byte[] _indexes;

        private DataBase(Header header, byte[] intervals, byte[] places, byte[] indexes)
        {
            _header = header;
            _intervals = intervals;
            _places = places;
            _indexes = indexes;
        }

        public static DataBase Load(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            using var stream = new MemoryStream(bytes);
            using var binaryReader = new BinaryReader(stream);
            var version = binaryReader.ReadInt32();
            var name = Encoding.UTF8.GetString(binaryReader.ReadBytes(32));
            var timestamp = binaryReader.ReadUInt64();
            var records = binaryReader.ReadInt32();
            var offsetRanges = binaryReader.ReadUInt32();
            var offsetCities = binaryReader.ReadUInt32();
            var offsetLocations = binaryReader.ReadUInt32();

            var header = new Header(version, name, timestamp, records, offsetRanges, offsetCities, offsetLocations);
            var intervals = binaryReader.ReadBytes(12 * records);
            var places = binaryReader.ReadBytes(96 * records);
            var indexes = binaryReader.ReadBytes(4 * records);
            return new DataBase(header, intervals, places, indexes);
        }

        public Place SearchPlaceByIp(string ip)
        {
            var intIp = ToUInt(ip);
            var (interval, index) = BinarySearch(_intervals, intIp, 12, new IpIntervalsComparer());
            var place = GetPlaceByIndex((int) interval.LocationIndex);
            return place;
        }

        private Place GetPlaceByIndex(int index)
        {
            var locationIndex = ToStruct<int>(_indexes, index * 4, 4);
            var place = ToStruct<Place>(_places, locationIndex, 96);
            return place;
        }

        public Place[] SearchPlacesByCity(string city)
        {
            var (place, index) = BinarySearchByIndex( _indexes, _places, city);
            if (index == null)
                return new Place[0];
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
            return res.ToArray();
        }

        public IpInterval[] GetAllIntervals()
        {
            using var stream = new MemoryStream(_intervals);
            using var binaryReader = new BinaryReader(stream);
            var intervals = new IpInterval[_header.Records];
            for (var i = 0; i < _header.Records; i++)
            {
                intervals[i] = ToStruct<IpInterval>(_intervals, i * 12, 12);
            }

            return intervals;
        }

        public Place[] GetAllPlaces()
        {
            using var stream = new MemoryStream(_places);
            using var binaryReader = new BinaryReader(stream);
            var places = new Place[_header.Records];
            for (var i = 0; i < _header.Records; i++)
            {
                places[i] = ToStruct<Place>(_places, i * 96, 96);
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

        private static uint ToUInt(string addr)
        {
            return (uint) IPAddress.NetworkToHostOrder(
                (int) IPAddress.Parse(addr).Address);
        }

        private static (T, int) BinarySearch<TKey, T>(byte[] inputArray, TKey key, int size,
            IShinyComparer<TKey, T> comparator)
        {
            var min = 0;
            var max = inputArray.Length / size - 1;
            while (min <= max)
            {
                var mid = (min + max) / 2;
                var structInterval = ToStruct<T>(inputArray, mid * size, size);
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

            return default;
        }
        
        private (Place, int?) BinarySearchByIndex(byte[] indexes, byte[] structs, string key)
        {
            var min = 0;
            var max = indexes.Length / 4 - 1;
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

        private static T ToStruct<T>(byte[] inputArray, int index, int size)
        {
            var sub = SubArray(inputArray, index, size);
            var structInterval = ByteArrayToStruct<T>(sub);
            return structInterval;
        }

        private static T ByteArrayToStruct<T>(byte[] bytes)
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                var stuff = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                return stuff;
            }
            finally
            {
                handle.Free();
            }
        }

        private static T[] SubArray<T>(T[] array, int offset, int length)
        {
            return new ArraySegment<T>(array, offset, length)
                .ToArray();
        }
    }
}