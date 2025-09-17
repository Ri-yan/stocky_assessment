
using Stocky.Api.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Stocky.Api.Repositories
{
    public class InMemoryHoldingRepository : IHoldingRepository
    {
        private readonly ConcurrentDictionary<string, Holding> _store = new();

        private string Key(Guid userId, string symbol) => $"{userId:N}:{symbol.ToUpperInvariant()}";

        public Task UpsertHoldingAsync(Holding h)
        {
            var key = Key(h.UserId, h.Symbol);
            _store.AddOrUpdate(key, h, (k, existing) => {
                // merge quantities & recompute weighted avg
                var totalQty = existing.Quantity + h.Quantity;
                decimal? newAvg = null;
                if (totalQty != 0)
                {
                    var existingValue = (existing.AvgPriceInr ?? 0m) * existing.Quantity;
                    var incomingValue = (h.AvgPriceInr ?? 0m) * h.Quantity;
                    newAvg = (existingValue + incomingValue) / totalQty;
                }
                existing.Quantity = totalQty;
                existing.AvgPriceInr = newAvg;
                existing.UpdatedAt = DateTimeOffset.UtcNow;
                return existing;
            });

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Holding>> GetHoldingsForUserAsync(Guid userId)
        {
            var res = _store.Values.Where(x => x.UserId == userId);
            return Task.FromResult(res);
        }

        public Task<Holding?> GetHoldingAsync(Guid userId, string symbol)
        {
            _store.TryGetValue(Key(userId, symbol), out var h);
            return Task.FromResult(h);
        }
    }
}
