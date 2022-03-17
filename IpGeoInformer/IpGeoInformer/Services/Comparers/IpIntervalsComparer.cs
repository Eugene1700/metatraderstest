using IpGeoInformer.Domain;

namespace IpGeoInformer.Services.Comparers
{
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