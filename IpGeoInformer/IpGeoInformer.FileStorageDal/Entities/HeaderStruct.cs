using System.Runtime.InteropServices;

namespace IpGeoInformer.FileStorageDal.Entities
{
    /// <summary>
    /// Заголовок
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 60, Pack = 1)]
    public readonly struct HeaderStruct
    {
        /// <summary>
        /// версия база данных
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public readonly int Version;
        
        /// <summary>
        /// название/префикс для базы данных
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
        public readonly string Name;
        
        /// <summary>
        /// время создания базы данных
        /// </summary>
        [MarshalAs(UnmanagedType.U8)]
        public readonly ulong TimeStamp;
        
        /// <summary>
        /// общее количество записей
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public readonly int Records;
        
        /// <summary>
        /// смещение относительно начала файла до начала списка записей с геоинформацией
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public readonly uint OffsetRanges;
        
        /// <summary>
        /// смещение относительно начала файла до начала индекса с сортировкой по названию городов
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public readonly uint OffsetCities;
        
        /// <summary>
        /// смещение относительно начала файла до начала списка записей о местоположении
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public readonly uint OffsetLocations;
    }
}