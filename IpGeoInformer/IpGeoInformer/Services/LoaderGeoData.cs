using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;

namespace IpGeoInformer
{
    public class LoaderGeoData
    {
        private readonly IMemoryCache _memoryCache;

        public LoaderGeoData(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
    }

    public interface IShinyComparer<in TKey, in T>
    {
        int Compare([AllowNull] TKey x, [AllowNull] T y);
    }

    public class IpIntervalsComparer : IShinyComparer<uint, IpInterval>
    {
        public int Compare(uint x, IpInterval y)
        {
            if (y.IpFrom <= x && y.IpTo >= x)
            {
                return 0;
            }

            if (y.IpFrom > x)
                return -1;

            return 1;
        }
    }
}