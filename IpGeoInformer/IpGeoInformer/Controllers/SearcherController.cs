using System;
using System.Diagnostics;
using System.Net;
using IpGeoInformer.Domain;
using IpGeoInformer.Domain.Model;
using IpGeoInformer.Domain.Services;
using IpGeoInformer.Helpers;
using IpGeoInformer.Models;
using IpGeoInformer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IpGeoInformer.Controllers
{
    [ApiController]
    public class SearcherController : ControllerBase
    {
        private readonly IGeoIpSearcher _geoIpSearcher;
        private readonly ILogger<SearcherController> _logger;

        public SearcherController(IGeoIpSearcher geoIpSearcher, ILogger<SearcherController> logger)
        {
            _geoIpSearcher = geoIpSearcher;
            _logger = logger;
        }

        [HttpGet]
        [Route("ip/location")]
        public Coordinates GetLocation([FromQuery] string ip)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var uintIp = ip.StrIpToUInt();
            var place = _geoIpSearcher.SearchPlaceByIp(uintIp);
            stopwatch.Stop();
            var stopwatchElapsed = stopwatch.Elapsed;
            _logger.LogInformation($"searchTime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            if (place == null)
                return null;
            return new Coordinates
            {
                Latitude = place.Latitude,
                Longitude = place.Longitude
            };
        }
        
        [HttpGet]
        [Route("city/locations")]
        public Place[] GetLocations([FromQuery] string city)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var res = _geoIpSearcher.SearchPlacesByCity(city);
            stopwatch.Stop();
            var stopwatchElapsed = stopwatch.Elapsed;
            _logger.LogInformation($"searchTime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            return res;
        }
    }
}