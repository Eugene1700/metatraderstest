using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using IpGeoInformer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IpGeoInformer
{
    
    public class DataLoaderService : IHostedService
    {
        private readonly ILogger<DataLoaderService> _logger;
        private readonly IServiceProvider _services;

        public DataLoaderService(ILogger<DataLoaderService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _services = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var filePath = @"C:\emm\metatraderstest\IpGeoInformer\IpGeoInformer.Tests\geobase.dat";
            using var scope = _services.CreateScope();
            var dataLoader = scope.ServiceProvider.GetRequiredService<GeoIpDataLoader>();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            dataLoader.Load(filePath);
            stopwatch.Stop();
            var stopwatchElapsed = stopwatch.Elapsed;
            _logger.LogInformation($"loadTime={Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}