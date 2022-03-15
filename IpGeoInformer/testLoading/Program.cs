using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using IpGeoInformer;

namespace testLoading
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            const string filePath = "geobase.dat";
            var db = DataBase.Load(filePath);
            stopwatch.Stop();
            var stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"loadTime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            
            var ip1 = DataBase.UInt32ToIpAddress(151738);
            var ip2 = DataBase.UInt32ToIpAddress(457815);
            
            stopwatch.Start();
            var r = db.SearchPlaceByIp(ip1.ToString());
            stopwatch.Stop();
            stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"res={r.Longitude}, {r.Latitude} searchtime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            
            stopwatch.Start();
            var r2 = db.SearchPlaceByIp(ip2.ToString());
            stopwatch.Stop();
            stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"res={r2.Longitude}, {r2.Latitude} searchtime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            
            stopwatch.Start();
            var places = db.SearchPlacesByCity("cit_Ageqenat");
            stopwatch.Stop();
            stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"res = {places.Length} searchtimeCity={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            
            stopwatch.Start();
            var places2 = db.SearchPlacesByCity("cit_Ahijinus L");
            stopwatch.Stop();
            stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"res = {places2.Length} searchtimeCity={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");

            stopwatch.Start();
            var places3 = db.SearchPlacesByCity("cit_Oze Ej Xa");
            stopwatch.Stop();
            stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"res = {places3.Length} searchtimeCity={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
        }
    }
}