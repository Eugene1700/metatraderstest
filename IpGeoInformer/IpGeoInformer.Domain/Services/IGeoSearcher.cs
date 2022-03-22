using IpGeoInformer.Domain.Model;

namespace IpGeoInformer.Domain.Services
{
    /// <summary>
    /// Интерфейс поиска Локаций
    /// </summary>
    public interface IGeoIpSearcher
    {
        /// <summary>
        /// Найти локации по ip
        /// </summary>
        /// <param name="ip">IP</param>
        /// <returns>Локация</returns>
        Location SearchLocationByIp(uint ip);
        
        /// <summary>
        /// Найти локации по точному совпадению названия города
        /// </summary>
        /// <param name="city">Название города</param>
        /// <returns>Локации</returns>
        Location[] SearchLocationsByCity(string city);
    }
}