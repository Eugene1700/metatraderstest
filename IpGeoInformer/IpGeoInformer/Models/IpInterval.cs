using System.Runtime.InteropServices;

namespace IpGeoInformer
{
    [StructLayout(LayoutKind.Sequential, Size = 12, Pack = 1)]
    public struct IpInterval
    {
        [MarshalAs(UnmanagedType.U4)] 
        // [FieldOffset(0)]
        public uint IpFrom;

        [MarshalAs(UnmanagedType.U4)] 
        // [FieldOffset(4)]
        public uint IpTo;

        [MarshalAs(UnmanagedType.U4)] 
        // [FieldOffset(8)]
        public uint LocationIndex;
    }
}