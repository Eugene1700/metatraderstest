using System;
using System.Runtime.InteropServices;

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
}