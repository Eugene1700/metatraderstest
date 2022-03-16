using System.Runtime.InteropServices;

namespace IpGeoInformer
{
    [StructLayout(LayoutKind.Sequential, Size = 96, Pack = 1)]
    public struct Place
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)] 
        // [FieldOffset(0)]
        public string Country;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] 
        // [FieldOffset(8)]
        public string Region;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] 
        // [FieldOffset(24)]
        public string Postal;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)] 
        // [FieldOffset(32)]
        public string City;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
        // [FieldOffset(56)]
        public string Organization;

        // [FieldOffset(88)]
        [MarshalAs(UnmanagedType.R4)]
        public float Latitude;

        // [FieldOffset(92)] 
        [MarshalAs(UnmanagedType.R4)]
        public float Longitude;
    }
}