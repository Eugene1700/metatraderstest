using System.Runtime.InteropServices;

namespace IpGeoInformer.FileStorageDal.Entities
{
    [StructLayout(LayoutKind.Sequential, Size = 60, Pack = 1)]
    public readonly struct HeaderStruct
    {
        [MarshalAs(UnmanagedType.I4)]
        public readonly int Version;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
        public readonly string Name;
        
        [MarshalAs(UnmanagedType.U8)]
        public readonly ulong TimeStamp;
        
        [MarshalAs(UnmanagedType.I4)]
        public readonly int Records;
        
        [MarshalAs(UnmanagedType.U4)]
        public readonly uint OffsetRanges;
        
        [MarshalAs(UnmanagedType.U4)]
        public readonly uint OffsetCities;
        
        [MarshalAs(UnmanagedType.U4)]
        public readonly uint OffsetLocations;
    }
}