using System.Runtime.InteropServices;
using IpGeoInformer.FileStorageDal.Entities;

namespace IpGeoInformer.FileStorageDal.Helpers
{
    public static class GeoIpDataBaseDescriptor
    {
        public static int HeaderSize => Marshal.SizeOf<HeaderStruct>();
        public static int IntervalsSize => Marshal.SizeOf<IpIntervalStruct>();
        public static int PlaceSize => Marshal.SizeOf<PlaceStruct>();
        public static int IndexSize => Marshal.SizeOf<int>();
        
        public const string HeaderKey = "header-key";
        public const string IntervalsKey = "intervals-key";
        public const string IndexesKey = "indexes-key";
        public const string PlacesKey = "places-key";
    }
}