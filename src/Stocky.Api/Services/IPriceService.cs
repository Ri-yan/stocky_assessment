
using System.Threading.Tasks;

namespace Stocky.Api.Services
{
    public interface IPriceService
    {
        /// <summary>
        /// Returns latest price for symbol in INR and a timestamp.
        /// </summary>
        Task<(decimal PriceInr, System.DateTimeOffset AsOf)> GetPriceAsync(string symbol);
    }
}
