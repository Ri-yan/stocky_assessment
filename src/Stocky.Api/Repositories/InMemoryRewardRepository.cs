
using Stocky.Api.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stocky.Api.Repositories
{
    public class InMemoryRewardRepository : IRewardRepository
    {
        private readonly ConcurrentDictionary<Guid, RewardEvent> _store = new();

        public Task AddAsync(RewardEvent ev)
        {
            _store[ev.Id] = ev;
            return Task.CompletedTask;
        }

        public Task<IEnumerable<RewardEvent>> GetByUserOnDateAsync(Guid userId, DateTimeOffset dayStart, DateTimeOffset dayEnd)
        {
            var res = _store.Values.Where(x => x.UserId == userId && x.RewardTimestamp >= dayStart && x.RewardTimestamp <= dayEnd);
            return Task.FromResult(res);
        }

        public Task<IEnumerable<RewardEvent>> GetByUserBeforeDateAsync(Guid userId, DateTimeOffset beforeDate)
        {
            var res = _store.Values.Where(x => x.UserId == userId && x.RewardTimestamp.Date < beforeDate.Date);
            return Task.FromResult(res);
        }

        public Task<RewardEvent?> GetByIdempotencyKeyAsync(string key)
        {
            var e = _store.Values.FirstOrDefault(x => x.IdempotencyKey == key);
            return Task.FromResult(e);
        }

        public Task<IEnumerable<RewardEvent>> GetAllForUserAsync(Guid userId)
        {
            var res = _store.Values.Where(x => x.UserId == userId);
            return Task.FromResult(res);
        }
    }
}
