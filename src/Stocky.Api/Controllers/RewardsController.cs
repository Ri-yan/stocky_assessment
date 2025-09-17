
using Microsoft.AspNetCore.Mvc;
using Stocky.Api.DTOs;
using Stocky.Api.Services;
using System.Threading.Tasks;

namespace Stocky.Api.Controllers
{
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly IRewardService _rewardService;
        public RewardsController(IRewardService rewardService)
        {
            _rewardService = rewardService;
        }

        [HttpPost("/reward")]
        public async Task<IActionResult> PostReward([FromBody] RewardRequest req)
        {
            if (req.Quantity <= 0) return BadRequest("Quantity must be > 0");
            var ev = await _rewardService.RewardAsync(req);
            return CreatedAtAction(nameof(GetReward), new { id = ev.Id }, ev);
        }

        [HttpGet("/reward/{id}")]
        public IActionResult GetReward([FromRoute] System.Guid id) => Ok(new { id });
    }
}
