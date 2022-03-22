using System.Runtime.InteropServices;
using IpGeoInformer.FileStorageDal.Entities;

namespace IpGeoInformer.FileStorageDal.Helpers
{
    /// <summary>
    /// Описатель файловой БД с данными о геопозиции
    /// </summary>
    public static class GeoIpDataBaseDescriptor
    {
        /// <summary>
        /// Размер заголовка
        /// </summary>
        public static int HeaderSize => Marshal.SizeOf<HeaderStruct>();
        /// <summary>
        /// Размер элемента ip-интервалов
        /// </summary>
        public static int IntervalsSize => Marshal.SizeOf<IpIntervalStruct>();
        /// <summary>
        /// Размер элемента локации
        /// </summary>
        public static int LocationSize => Marshal.SizeOf<LocationStruct>();
        /// <summary>
        /// Размер элемента индекса
        /// </summary>
        public static int IndexSize => Marshal.SizeOf<int>();
        
        /// <summary>
        /// Ключ в кэша для залоговка
        /// </summary>
        public const string HeaderKey = "header-key";
        /// <summary>
        /// Ключ в кэша для ip-интервалов
        /// </summary>
        public const string IntervalsKey = "intervals-key";
        /// <summary>
        /// Ключ в кэша для индексов
        /// </summary>
        public const string IndexesKey = "indexes-key";
        /// <summary>
        /// Ключ в кэша для локаций
        /// </summary>
        public const string LocationsKey = "locations-key";
    }
}