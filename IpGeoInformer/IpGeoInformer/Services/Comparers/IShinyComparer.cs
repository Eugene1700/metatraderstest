using System.Diagnostics.CodeAnalysis;

namespace IpGeoInformer.Services.Comparers
{
    public interface IShinyComparer<in TKey, in T>
    {
        int Compare([AllowNull] TKey x, [AllowNull] T y);
    }
}