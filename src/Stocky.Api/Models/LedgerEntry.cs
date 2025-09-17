
using System;

namespace Stocky.Api.Models
{
    public class LedgerEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? EventId { get; set; }
        public string EntryType { get; set; } = null!; // STOCK_UNIT | CASH_OUTFLOW | FEE
        public string Account { get; set; } = null!;
        public string? Symbol { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? AmountInr { get; set; } // NUMERIC(18,4)
        public char DebitCredit { get; set; } // 'D' or 'C'
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
