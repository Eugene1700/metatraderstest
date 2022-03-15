using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace IpGeoInformer
{
    public class LoaderGeoData
    {
        private readonly IMemoryCache _memoryCache;

        public LoaderGeoData(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
    }

    public class Header
    {
        public Header(int version, string name, ulong timeStamp, int records, uint offsetRanges, uint offsetCities,
            uint offsetLocations)
        {
            Version = version;
            Name = name;
            TimeStamp = timeStamp;
            Records = records;
            OffsetRanges = offsetRanges;
            OffsetCities = offsetCities;
            OffsetLocations = offsetLocations;
        }

        public int Version { get; }

        public string Name { get; }

        public ulong TimeStamp { get; }

        public int Records { get; }

        public uint OffsetRanges { get; }

        public uint OffsetCities { get; }

        public uint OffsetLocations { get; }
    }

    [StructLayout(LayoutKind.Explicit, Size = 12, Pack = 1)]
    public struct IpInterval
    {
        [MarshalAs(UnmanagedType.U4)] [FieldOffset(0)]
        public uint IpFrom;

        [MarshalAs(UnmanagedType.U4)] [FieldOffset(4)]
        public uint IpTo;

        [MarshalAs(UnmanagedType.U4)] [FieldOffset(8)]
        public uint LocationIndex;
    }

    [StructLayout(LayoutKind.Explicit, Size = 96, Pack = 1)]
    public struct Place
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)] [FieldOffset(0)]
        public string Country;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] [FieldOffset(8)]
        public string Region;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] [FieldOffset(24)]
        public string Postal;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)] [FieldOffset(32)]
        public string City;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] [FieldOffset(56)]
        public string Organization;

        [FieldOffset(88)] public float Latitude;

        [FieldOffset(92)] public float Longitude;
    }

    public interface IShinyComparer<in TKey, T>
    {
        int Compare([AllowNull] TKey x, [AllowNull] T y);
    }

    public class IpIntervalsComparer : IShinyComparer<uint, IpInterval>
    {
        public int Compare(uint x, IpInterval y)
        {
            if (y.IpFrom <= x && y.IpTo >= x)
            {
                return 0;
            }

            if (y.IpFrom > x)
                return -1;

            return 1;
        }
    }

    public class CityComparer : IShinyComparer<string, Place>
    {
        public int Compare(string x, Place y)
        {
            return x.CompareTo(y.City);
        }
    }

    public class IndexComparer : IShinyComparer<int, int>
    {
        public int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }
    }
}