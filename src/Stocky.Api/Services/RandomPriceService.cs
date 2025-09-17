
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Stocky.Api.Services
{
    /// <summary>
    /// Simulated price service that keeps an in-memory price per symbol and updates it randomly.
    /// Used so the app can calculate INR valuations without external API calls.
    /// </summary>
    public class RandomPriceService : IPriceService
    {
        private readonly ConcurrentDictionary<string, (decimal price, DateTimeOffset asOf)> _prices = new();

        private readonly Random _rng = new();
        public RandomPriceService()
        {
            // seed some sample symbols
            var now = DateTimeOffset.UtcNow;
            _prices["RELIANCE"] = (2500.00m, now);
            _prices["TCS"] = (3200.00m, now);
            _prices["INFY"] = (1500.00m, now);
        }

        public Task<(decimal PriceInr, DateTimeOffset AsOf)> GetPriceAsync(string symbol)
        {
            var sym = symbol.ToUpperInvariant();
            var exists = _prices.GetOrAdd(sym, (100m, DateTimeOffset.UtcNow));
            // simulate small random drift
            var change = (decimal)(_rng.NextDouble() - 0.5) * 10m; // +/-5 INR approx
            var newPrice = Math.Max(1m, Math.Round(exists.price + change, 2));
            var asOf = DateTimeOffset.UtcNow;
            _prices[sym] = (newPrice, asOf);
            return Task.FromResult((newPrice, asOf));
        }
    }
}
