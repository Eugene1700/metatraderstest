using System;
using System.Runtime.InteropServices;
using IpGeoInformer.Services;
using IpGeoInformer.Services.Comparers;

namespace IpGeoInformer.Helpers
{
    public static class ArrayExtensions
    {
        public static T ToStruct<T>( this byte[] inputArray, int index)
        {
            var size = Marshal.SizeOf<T>();
            var sub = SubArray(inputArray, index, size);
            var structInterval = ByteArrayToStruct<T>(sub);
            return structInterval;
        }

        public static SearchResult<TSearchStruct> BinarySearch<TSearchKey, TSearchStruct>(this byte[] inputArray, 
            TSearchKey key,
            int itemSize,
            Func<int, TSearchStruct> toStruct,
            IShinyComparer<TSearchKey, TSearchStruct> comparator)
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
        
        private static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            return new ArraySegment<T>(array, offset, length)
                .ToArray();
        }
        
        private static T ByteArrayToStruct<T>(byte[] bytes)
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

    public class SearchResult<T>
    {
        public T Result { get; set; }
        public int Index { get; set; }
        
        public void Deconstruct(out T result, out int index)
        {
            result = Result;
            index = Index;
        }
    }
}