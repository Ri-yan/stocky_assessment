
using System;
using System.Threading.Tasks;
using Stocky.Api.DTOs;

namespace Stocky.Api.Services
{
    public interface IStatsService
    {
        Task<object> GetStatsAsync(Guid userId);
    }
}
