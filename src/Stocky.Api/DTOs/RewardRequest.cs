
using System;

namespace Stocky.Api.DTOs
{
    public class RewardRequest
    {
        public Guid UserId { get; set; }
        public string Symbol { get; set; } = null!;
        public decimal Quantity { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string? IdempotencyKey { get; set; }
        public string? Metadata { get; set; }
    }
}
