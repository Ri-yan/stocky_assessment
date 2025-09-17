
namespace Stocky.Api.DTOs
{
    public class PortfolioItemDto
    {
        public string Symbol { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal CurrentPriceInr { get; set; }
        public decimal CurrentValueInr { get; set; }
    }
}
