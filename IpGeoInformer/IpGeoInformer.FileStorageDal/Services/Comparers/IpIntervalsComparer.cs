using IpGeoInformer.Domain;
using IpGeoInformer.FileStorageDal.Entities;
using IpGeoInformer.Services.Comparers;

namespace IpGeoInformer.FileStorageDal.Services.Comparers
{
    public class IpIntervalsComparer : IShinyComparer<uint, IpIntervalStruct>
    {
        public int Compare(uint x, IpIntervalStruct y)
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