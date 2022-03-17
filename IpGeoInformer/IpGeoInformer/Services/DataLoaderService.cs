using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using IpGeoInformer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IpGeoInformer
{
    
    public class DataLoaderService : IHostedService
    {
        private readonly ILogger<DataLoaderService> _logger;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        public DataLoaderService(ILogger<DataLoaderService> logger,
            IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _services = serviceProvider;
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var filePath = _configuration["database"];
            using var scope = _services.CreateScope();
            var dataLoader = scope.ServiceProvider.GetRequiredService<GeoIpDataLoader>();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            dataLoader.Load(filePath);
            stopwatch.Stop();
            var stopwatchElapsed = stopwatch.Elapsed;
            _logger.LogInformation($"dbloading time=[{Convert.ToInt32(stopwatchElapsed.TotalMilliseconds)} ms]");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}