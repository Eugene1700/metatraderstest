using System.Runtime.InteropServices;

namespace IpGeoInformer.Domain
{
    [StructLayout(LayoutKind.Sequential, Size = 96, Pack = 1)]
    public struct Place
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)] 
        public readonly string Country;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] 
        public readonly string Region;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] 
        public readonly string Postal;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)] 
        public readonly string City;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
        public readonly string Organization;

        [MarshalAs(UnmanagedType.R4)]
        public readonly float Latitude;

        [MarshalAs(UnmanagedType.R4)]
        public readonly float Longitude;
    }
}