using System.Runtime.InteropServices;

namespace IpGeoInformer.FileStorageDal.Entities
{
    [StructLayout(LayoutKind.Sequential, Size = 12, Pack = 1)]
    public readonly struct IpIntervalStruct
    {
        [MarshalAs(UnmanagedType.U4)] 
        public readonly uint IpFrom;

        [MarshalAs(UnmanagedType.U4)] 
        public readonly uint IpTo;

        [MarshalAs(UnmanagedType.U4)] 
        public readonly uint LocationIndex;
    }
}