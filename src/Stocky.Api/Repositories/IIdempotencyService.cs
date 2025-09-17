
using System.Threading.Tasks;

namespace Stocky.Api.Repositories
{
    public interface IIdempotencyService
    {
        Task<bool> TryRegisterKeyAsync(string key);
    }
}
