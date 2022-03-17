using System.Runtime.InteropServices;

namespace IpGeoInformer.Domain
{
    [StructLayout(LayoutKind.Sequential, Size = 12, Pack = 1)]
    public struct IpInterval
    {
        [MarshalAs(UnmanagedType.U4)] 
        public uint IpFrom;

        [MarshalAs(UnmanagedType.U4)] 
        public uint IpTo;

        [MarshalAs(UnmanagedType.U4)] 
        public uint LocationIndex;
    }
}