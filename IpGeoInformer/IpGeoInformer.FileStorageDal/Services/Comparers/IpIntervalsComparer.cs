using IpGeoInformer.Domain;
using IpGeoInformer.FileStorageDal.Entities;

namespace IpGeoInformer.FileStorageDal.Services.Comparers
{
    public class IpIntervalsComparer : IKeyComparer<uint, IpIntervalStruct>
    {
        public int Compare(uint key, IpIntervalStruct y)
        {
            if (y.IpFrom <= key && y.IpTo >= key)
            {
                return 0;
            }

            if (y.IpFrom > key)
                return -1;

            return 1;
        }
    }
}