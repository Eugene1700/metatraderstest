using IpGeoInformer.Domain;
using IpGeoInformer.FileStorageDal.Entities;
using IpGeoInformer.Services.Comparers;

namespace IpGeoInformer.FileStorageDal.Services.Comparers
{
    public class CityComparer : IShinyComparer<string, PlaceStruct>
    {
        public int Compare(string x, PlaceStruct y)
        {
            return x.CompareTo(y.City);
        }
    }
}