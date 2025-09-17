
using Stocky.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocky.Api.Repositories
{
    public interface IRewardRepository
    {
        Task AddAsync(RewardEvent ev);
        Task<IEnumerable<RewardEvent>> GetByUserOnDateAsync(Guid userId, DateTimeOffset dayStart, DateTimeOffset dayEnd);
        Task<IEnumerable<RewardEvent>> GetByUserBeforeDateAsync(Guid userId, DateTimeOffset beforeDate);
        Task<RewardEvent?> GetByIdempotencyKeyAsync(string key);
        Task<IEnumerable<RewardEvent>> GetAllForUserAsync(Guid userId);
    }
}
