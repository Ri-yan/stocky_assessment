
using System;

namespace Stocky.Api.Models
{
    public class RewardEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Symbol { get; set; } = null!;
        public decimal Quantity { get; set; } // NUMERIC(18,6) semantically
        public DateTimeOffset RewardTimestamp { get; set; }
        public string? IdempotencyKey { get; set; }
        public string? Metadata { get; set; }
    }
}
