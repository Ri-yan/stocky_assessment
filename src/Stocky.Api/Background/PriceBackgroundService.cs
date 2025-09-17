
using Microsoft.Extensions.Hosting;
using Stocky.Api.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Stocky.Api.Background
{
    /// <summary>
    /// Background service that periodically refreshes prices (simulated).
    /// In production this would call an external price provider and update a cache.
    /// </summary>
    public class PriceBackgroundService : BackgroundService
    {
        private readonly IPriceService _priceService;
        private readonly ILogger<PriceBackgroundService> _logger;

        public PriceBackgroundService(IPriceService priceService, ILogger<PriceBackgroundService> logger)
        {
            _priceService = priceService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PriceBackgroundService started");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Request price for known symbols to cause simulated drift
                    await _priceService.GetPriceAsync("RELIANCE");
                    await _priceService.GetPriceAsync("TCS");
                    await _priceService.GetPriceAsync("INFY");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating simulated prices");
                }
                // Sleep ~1 minute for demo (in prod this would be hourly)
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
