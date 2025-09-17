
using Stocky.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocky.Api.Repositories
{
    public interface ILedgerRepository
    {
        Task AddAsync(LedgerEntry entry);
        Task<IEnumerable<LedgerEntry>> GetByEventIdAsync(Guid eventId);
        Task<IEnumerable<LedgerEntry>> GetAllAsync();
    }
}
