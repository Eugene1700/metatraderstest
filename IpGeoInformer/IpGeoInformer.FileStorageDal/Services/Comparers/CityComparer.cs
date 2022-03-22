using IpGeoInformer.FileStorageDal.Entities;

namespace IpGeoInformer.FileStorageDal.Services.Comparers
{
    /// <summary>
    /// Сравнение для названий городов
    /// </summary>
    public class CityComparer : IKeyComparer<string, LocationStruct>
    {
        /// <inheritdoc />
        public int Compare(string key, LocationStruct y)
        {
            return key.CompareTo(y.City);
        }
    }
}