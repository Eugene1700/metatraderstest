using System.Runtime.InteropServices;

namespace IpGeoInformer.Domain
{
    [StructLayout(LayoutKind.Sequential, Size = 60, Pack = 1)]
    public struct Header
    {
        [MarshalAs(UnmanagedType.I4)]
        public int Version;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
        public string Name;
        
        [MarshalAs(UnmanagedType.U8)]
        public ulong TimeStamp;
        
        [MarshalAs(UnmanagedType.I4)]
        public int Records;
        
        [MarshalAs(UnmanagedType.U4)]
        public uint OffsetRanges;
        
        [MarshalAs(UnmanagedType.U4)]
        public uint OffsetCities;
        
        [MarshalAs(UnmanagedType.U4)]
        public uint OffsetLocations;
    }
}