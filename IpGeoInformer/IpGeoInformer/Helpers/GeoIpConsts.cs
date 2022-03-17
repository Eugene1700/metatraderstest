using System.Runtime.InteropServices;
using IpGeoInformer.Domain;

namespace IpGeoInformer.Helpers
{
    public static class GeoIpConsts
    {
        public static int HeaderSize => Marshal.SizeOf<Header>();
        public static int IntervalsSize => Marshal.SizeOf<IpInterval>();
        public static int PlaceSize => Marshal.SizeOf<Place>();
        public static int IndexSize => Marshal.SizeOf<int>();
        
        public const string HeaderKey = "header-key";
        public const string IntervalsKey = "intervals-key";
        public const string IndexesKey = "indexes-key";
        public const string PlacesKey = "places-key";
    }
}