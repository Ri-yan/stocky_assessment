
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Stocky.Api.Repositories
{
    public class InMemoryIdempotencyService : IIdempotencyService
    {
        private readonly ConcurrentDictionary<string, byte> _keys = new();

        public Task<bool> TryRegisterKeyAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) return Task.FromResult(true); // no key => no dedupe
            var added = _keys.TryAdd(key, 1);
            return Task.FromResult(added);
        }
    }
}
