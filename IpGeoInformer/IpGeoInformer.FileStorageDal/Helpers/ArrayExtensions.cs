using System;
using System.Runtime.InteropServices;
using IpGeoInformer.FileStorageDal.Services.Comparers;

namespace IpGeoInformer.FileStorageDal.Helpers
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Выделить структуру из массива байт
        /// </summary>
        /// <param name="inputArray">Массив</param>
        /// <param name="index">Индекс в исходном массиве</param>
        /// <typeparam name="T">Тип структуры</typeparam>
        /// <returns>Структура</returns>
        public static T ToStruct<T>(this byte[] inputArray, int index) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var subArray = inputArray.SubArray(index, size);
            var @struct = subArray.ByteArrayToStruct<T>();
            return @struct;
        }

        /// <summary>
        /// Бинарный поиск по байтовому массиву
        /// </summary>
        /// <param name="inputArray">Массив</param>
        /// <param name="key">Ключ для поиска</param>
        /// <param name="itemSize">Размер структуры в массиве</param>
        /// <param name="toStruct">Метод для овеществления структуры</param>
        /// <param name="comparator">Компаратор</param>
        /// <typeparam name="TSearchKey">Тип ключа для поиска</typeparam>
        /// <typeparam name="TSearchStruct">Тип структуры</typeparam>
        /// <returns>Результат: индекс и искомый объект или null</returns>
        public static SearchResult<TSearchStruct> BinarySearch<TSearchKey, TSearchStruct>(this byte[] inputArray,
            TSearchKey key,
            int itemSize,
            Func<int, TSearchStruct> toStruct,
            IKeyComparer<TSearchKey, TSearchStruct> comparator) where TSearchStruct : struct
        {
            var min = 0;
            var max = inputArray.Length / itemSize - 1;
            while (min <= max)
            {
                var mid = (min + max) / 2;
                var candidate = toStruct(mid);
                var compRes = comparator.Compare(key, candidate);
                if (compRes == 0)
                {
                    return new SearchResult<TSearchStruct>
                    {
                        Result = candidate,
                        Index = mid
                    };
                }

                if (compRes < 0)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }

            return null;
        }

        /// <summary>
        /// выделить подмассив
        /// </summary>
        /// <param name="array">Массив</param>
        /// <param name="offset">Сдвиг относительно начала</param>
        /// <param name="length">Размер подмассива</param>
        /// <typeparam name="T">Тип массива</typeparam>
        /// <returns>Подмассив</returns>
        private static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            return new ArraySegment<T>(array, offset, length)
                .ToArray();
        }

        /// <summary>
        /// Преобразовать массив байт в структуру
        /// </summary>
        /// <param name="bytes">Массив</param>
        /// <typeparam name="T">Тип структуры</typeparam>
        /// <returns>Структура</returns>
        private static T ByteArrayToStruct<T>(this byte[] bytes)
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                var stuff = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                return stuff;
            }
            finally
            {
                handle.Free();
            }
        }
    }

    /// <summary>
    /// Результат поиска объекта
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    public class SearchResult<T>
    {
        /// <summary>
        /// Искомый объект
        /// </summary>
        public T Result { get; set; }
        
        /// <summary>
        /// Индекс в массиве
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Деконструктор
        /// </summary>
        /// <param name="result">Объект</param>
        /// <param name="index">Индекс</param>
        public void Deconstruct(out T result, out int index)
        {
            result = Result;
            index = Index;
        }
    }
}