
using Stocky.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocky.Api.Repositories
{
    public interface IHoldingRepository
    {
        Task UpsertHoldingAsync(Holding h);
        Task<IEnumerable<Holding>> GetHoldingsForUserAsync(Guid userId);
        Task<Holding?> GetHoldingAsync(Guid userId, string symbol);
    }
}
