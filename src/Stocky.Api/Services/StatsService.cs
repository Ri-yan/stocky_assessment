
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stocky.Api.Repositories;

namespace Stocky.Api.Services
{
    public class StatsService : IStatsService
    {
        private readonly IRewardRepository _rewardRepo;
        private readonly IPortfolioService _portfolioService;
        public StatsService(IRewardRepository rewardRepo, IPortfolioService portfolioService)
        {
            _rewardRepo = rewardRepo;
            _portfolioService = portfolioService;
        }

        public async Task<object> GetStatsAsync(Guid userId)
        {
            var start = DateTimeOffset.UtcNow.Date;
            var end = start.AddDays(1).AddTicks(-1);
            var todays = await _rewardRepo.GetByUserOnDateAsync(userId, start, end);
            var grouped = todays.GroupBy(x => x.Symbol).ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            var portfolio = await _portfolioService.GetPortfolioAsync(userId);
            var totalValue = portfolio.Sum(x => x.CurrentValueInr);

            return new {
                totalSharesRewardedToday = grouped,
                currentPortfolioValueInr = Math.Round(totalValue, 4)
            };
        }
    }
}
