using IpGeoInformer.Domain;

namespace IpGeoInformer.Services.Comparers
{
    public class CityComparer : IShinyComparer<string, Place>
    {
        public int Compare(string x, Place y)
        {
            return x.CompareTo(y.City);
        }
    }
}