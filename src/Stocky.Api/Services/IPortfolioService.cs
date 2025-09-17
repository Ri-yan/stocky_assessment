
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stocky.Api.DTOs;

namespace Stocky.Api.Services
{
    public interface IPortfolioService
    {
        Task<IEnumerable<PortfolioItemDto>> GetPortfolioAsync(Guid userId);
    }
}
