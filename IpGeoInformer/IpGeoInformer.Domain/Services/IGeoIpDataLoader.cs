namespace IpGeoInformer.Domain.Services
{
    /// <summary>
    /// Загручик данных
    /// </summary>
    public interface IGeoIpDataLoader
    {
        /// <summary>
        /// Загрзуить из файла
        /// </summary>
        /// <param name="filePath">Абсолютный путь до файла</param>
        void Load(string filePath);
    }
}