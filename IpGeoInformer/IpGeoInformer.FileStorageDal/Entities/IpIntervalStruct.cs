using System.Runtime.InteropServices;

namespace IpGeoInformer.FileStorageDal.Entities
{
    /// <summary>
    /// IP-интервалы
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 12, Pack = 1)]
    public readonly struct IpIntervalStruct
    {
        /// <summary>
        /// начало диапазона IP адресов
        /// </summary>
        [MarshalAs(UnmanagedType.U4)] 
        public readonly uint IpFrom;

        /// <summary>
        /// конец диапазона IP адресов
        /// </summary>
        [MarshalAs(UnmanagedType.U4)] 
        public readonly uint IpTo;

        /// <summary>
        /// индекс записи о местоположении
        /// </summary>
        [MarshalAs(UnmanagedType.U4)] 
        public readonly uint LocationIndex;
    }
}