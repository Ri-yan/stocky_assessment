
using Stocky.Api.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Stocky.Api.Repositories
{
    public class InMemoryLedgerRepository : ILedgerRepository
    {
        private readonly ConcurrentDictionary<Guid, LedgerEntry> _store = new();

        public Task AddAsync(LedgerEntry entry)
        {
            _store[entry.Id] = entry;
            return Task.CompletedTask;
        }

        public Task<IEnumerable<LedgerEntry>> GetByEventIdAsync(Guid eventId)
        {
            var res = _store.Values.Where(x => x.EventId == eventId);
            return Task.FromResult(res);
        }

        public Task<IEnumerable<LedgerEntry>> GetAllAsync()
        {
            return Task.FromResult(_store.Values.AsEnumerable());
        }
    }
}
