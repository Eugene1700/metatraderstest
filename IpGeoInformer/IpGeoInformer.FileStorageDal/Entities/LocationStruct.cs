using System.Runtime.InteropServices;

namespace IpGeoInformer.FileStorageDal.Entities
{
    /// <summary>
    /// Структура для локации
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 96, Pack = 1)]
    public readonly struct LocationStruct
    {
        /// <summary>
        /// Страна
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)] 
        public readonly string Country;

        /// <summary>
        /// Регион
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] 
        public readonly string Region;

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] 
        public readonly string Postal;

        /// <summary>
        /// Город
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)] 
        public readonly string City;

        /// <summary>
        /// Организация
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] 
        public readonly string Organization;

        /// <summary>
        /// Широта
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public readonly float Latitude;

        /// <summary>
        /// Долгота
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public readonly float Longitude;
    }
}