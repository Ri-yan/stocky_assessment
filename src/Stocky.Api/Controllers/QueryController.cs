using Microsoft.AspNetCore.Mvc;
using Stocky.Api.Repositories;
using Stocky.Api.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stocky.Api.Controllers
{
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IRewardRepository _rewardRepo;
        private readonly IPortfolioService _portfolioService;
        private readonly IStatsService _statsService;

        public QueryController(IRewardRepository rewardRepo, IPortfolioService portfolioService, IStatsService statsService)
        {
            _rewardRepo = rewardRepo;
            _portfolioService = portfolioService;
            _statsService = statsService;
        }

        [HttpGet("/today-stocks/{userId}")]
        public async Task<IActionResult> TodayStocks([FromRoute] Guid userId)
        {
            var start = DateTimeOffset.UtcNow.Date;
            var end = start.AddDays(1).AddTicks(-1);
            var todays = await _rewardRepo.GetByUserOnDateAsync(userId, start, end);

            var grouped = todays
                .GroupBy(x => x.Symbol)
                .Select(g => new { Symbol = g.Key, Quantity = g.Sum(x => x.Quantity) });

            return Ok(grouped);
        }

        [HttpGet("/historical-inr/{userId}")]
        public async Task<IActionResult> HistoricalInr([FromRoute] Guid userId)
        {
            // Compute daily portfolio INR values up to yesterday
            var beforeDate = DateTimeOffset.UtcNow.Date;
            var events = await _rewardRepo.GetByUserBeforeDateAsync(userId, beforeDate);

            var daily = events
     .GroupBy(e => e.RewardTimestamp.Date)
     .Select(g => new
     {
         Date = g.Key,
         TotalQuantity = g.Sum(ev => ev.Quantity)
     })
     .OrderBy(x => x.Date)
     .ToList();


            return Ok(daily);
        }

        [HttpGet("/stats/{userId}")]
        public async Task<IActionResult> Stats([FromRoute] Guid userId)
        {
            var s = await _statsService.GetStatsAsync(userId);
            return Ok(s);
        }

        [HttpGet("/portfolio/{userId}")]
        public async Task<IActionResult> Portfolio([FromRoute] Guid userId)
        {
            var p = await _portfolioService.GetPortfolioAsync(userId);
            return Ok(p);
        }
    }
}
