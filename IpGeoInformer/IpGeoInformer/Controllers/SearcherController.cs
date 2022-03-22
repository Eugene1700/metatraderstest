using System;
using System.Diagnostics;
using IpGeoInformer.Domain.Model;
using IpGeoInformer.Domain.Services;
using IpGeoInformer.Helpers;
using IpGeoInformer.Models;
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

        /// <summary>
        /// Получить координаты по ip
        /// </summary>
        /// <param name="ip">IP-адрес</param>
        /// <returns>Координаты</returns>
        [HttpGet]
        [Route("ip/location")]
        public Coordinates GetLocation([FromQuery] string ip)
        {
            var uintIp = ip.StrIpToUInt();
            var place = _geoIpSearcher.SearchLocationByIp(uintIp);
            if (place == null)
                return null;
            return new Coordinates
            {
                Latitude = place.Latitude,
                Longitude = place.Longitude
            };
        }
        
        /// <summary>
        /// Найти локации по названию городу
        /// </summary>
        /// <param name="city">Название города</param>
        /// <returns>Локации</returns>
        [HttpGet]
        [Route("city/locations")]
        public Location[] GetLocations([FromQuery] string city)
        {
            return _geoIpSearcher.SearchLocationsByCity(city);
        }
    }
}