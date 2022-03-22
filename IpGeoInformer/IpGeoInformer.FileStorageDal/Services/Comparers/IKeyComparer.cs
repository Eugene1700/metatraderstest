using System.Diagnostics.CodeAnalysis;

namespace IpGeoInformer.FileStorageDal.Services.Comparers
{
    /// <summary>
    /// Интерфейс для сравнения объектов по ключу
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="T">Тип объекта</typeparam>
    public interface IKeyComparer<in TKey, in T>
    {
        /// <summary>
        /// Сравнить
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="obj">Объект для сравнения</param>
        /// <returns></returns>
        int Compare([AllowNull] TKey key, [AllowNull] T obj);
    }
}