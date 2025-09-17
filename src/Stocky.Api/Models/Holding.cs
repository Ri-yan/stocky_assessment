
using System;

namespace Stocky.Api.Models
{
    public class Holding
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Symbol { get; set; } = null!;
        public decimal Quantity { get; set; } // NUMERIC(18,6)
        public decimal? AvgPriceInr { get; set; } // NUMERIC(18,4)
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
