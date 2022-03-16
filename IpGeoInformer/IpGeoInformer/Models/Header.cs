using System.Runtime.InteropServices;

namespace IpGeoInformer
{
    [StructLayout(LayoutKind.Sequential, Size = 60, Pack = 1)]
    public struct Header
    {
        [MarshalAs(UnmanagedType.I4)]
        // [FieldOffset(0)] 
        public int Version;
        
        // [FieldOffset(4)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
        public string Name;
        
        [MarshalAs(UnmanagedType.U8)]
        // [FieldOffset(36)] 
        public ulong TimeStamp;
        
        [MarshalAs(UnmanagedType.I4)]
        // [FieldOffset(44)] 
        public int Records;
        
        [MarshalAs(UnmanagedType.U4)]
        // [FieldOffset(48)] 
        public uint OffsetRanges;
        
        [MarshalAs(UnmanagedType.U4)]
        // [FieldOffset(52)] 
        public uint OffsetCities;
        
        [MarshalAs(UnmanagedType.U4)]
        // [FieldOffset(56)] 
        public uint OffsetLocations;
    }
}