
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stocky.Api.DTOs;
using Stocky.Api.Repositories;

namespace Stocky.Api.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IHoldingRepository _holdingRepo;
        private readonly IPriceService _priceService;
        public PortfolioService(IHoldingRepository holdingRepo, IPriceService priceService)
        {
            _holdingRepo = holdingRepo;
            _priceService = priceService;
        }

        public async Task<IEnumerable<PortfolioItemDto>> GetPortfolioAsync(Guid userId)
        {
            var holdings = await _holdingRepo.GetHoldingsForUserAsync(userId);
            var list = new List<PortfolioItemDto>();
            foreach (var h in holdings)
            {
                var (price, asOf) = await _priceService.GetPriceAsync(h.Symbol);
                var value = Math.Round(h.Quantity * price, 4);
                list.Add(new PortfolioItemDto {
                    Symbol = h.Symbol,
                    Quantity = h.Quantity,
                    CurrentPriceInr = price,
                    CurrentValueInr = value
                });
            }
            return list;
        }
    }
}
