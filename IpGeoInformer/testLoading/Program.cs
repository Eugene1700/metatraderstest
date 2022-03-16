using System;
using System.Diagnostics;
using IpGeoInformer;
using IpGeoInformer.Services;
using Microsoft.Extensions.Caching.Memory;

namespace testLoading
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            const string filePath = "geobase.dat";
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var dataLoader = new GeoIpDataLoader(memoryCache);
            dataLoader.Load(filePath);
            stopwatch.Stop();
            var stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"loadTime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            
            var ip1 = GeoIpDataProvider.UInt32ToIpAddress(151738);
            var ip2 = GeoIpDataProvider.UInt32ToIpAddress(457815);
            
            var db = new GeoIpDataProvider(memoryCache);
            stopwatch.Start();
            var r = db.SearchPlaceByIp(ip1.ToString());
            stopwatch.Stop();
            stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"res={r.Longitude}, {r.Latitude} searchtime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            
            stopwatch.Start();
            var rRep = db.SearchPlaceByIp(ip1.ToString());
            stopwatch.Stop();
            stopwatchElapsed = stopwatch.Elapsed;
            Console.WriteLine($"resrRep={rRep.Longitude}, {rRep.Latitude} searchtime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");

            
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