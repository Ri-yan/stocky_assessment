
using System;
using System.Threading.Tasks;
using Stocky.Api.DTOs;
using Stocky.Api.Models;

namespace Stocky.Api.Services
{
    public interface IRewardService
    {
        Task<RewardEvent> RewardAsync(RewardRequest req);
    }
}
