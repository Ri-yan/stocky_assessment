
using System;
using System.Threading.Tasks;
using Stocky.Api.DTOs;
using Stocky.Api.Models;
using Stocky.Api.Repositories;

namespace Stocky.Api.Services
{
    public class RewardService : IRewardService
    {
        private readonly IRewardRepository _rewardRepo;
        private readonly ILedgerRepository _ledgerRepo;
        private readonly IHoldingRepository _holdingRepo;
        private readonly IPriceService _priceService;
        private readonly IIdempotencyService _idempotency;

        public RewardService(IRewardRepository rewardRepo, ILedgerRepository ledgerRepo, IHoldingRepository holdingRepo, IPriceService priceService, IIdempotencyService idempotency)
        {
            _rewardRepo = rewardRepo;
            _ledgerRepo = ledgerRepo;
            _holdingRepo = holdingRepo;
            _priceService = priceService;
            _idempotency = idempotency;
        }

        public async Task<RewardEvent> RewardAsync(RewardRequest req)
        {
            // Idempotency: if key provided and already seen -> return existing event (simple behavior)
            if (!string.IsNullOrEmpty(req.IdempotencyKey))
            {
                var registered = await _idempotency.TryRegisterKeyAsync(req.IdempotencyKey);
                if (!registered)
                {
                    var existing = await _rewardRepo.GetByIdempotencyKeyAsync(req.IdempotencyKey);
                    if (existing != null) return existing;
                }
            }

            var ev = new RewardEvent
            {
                UserId = req.UserId,
                Symbol = req.Symbol.ToUpperInvariant(),
                Quantity = decimal.Round(req.Quantity, 6),
                RewardTimestamp = req.Timestamp,
                IdempotencyKey = req.IdempotencyKey,
                Metadata = req.Metadata
            };

            // Get price to compute INR cost to company (including fees)
            var (price, asOf) = await _priceService.GetPriceAsync(ev.Symbol);
            var marketValue = ev.Quantity * price;

            // Fees (simulated)
            var brokerage = Math.Round(marketValue * 0.001m, 4); // 0.1%
            var stt = Math.Round(marketValue * 0.0005m, 4); // 0.05%
            var gst = Math.Round((brokerage) * 0.18m, 4); // GST on brokerage
            var totalFees = brokerage + stt + gst;
            var companyOutflow = Math.Round(marketValue + totalFees, 4);

            // Create ledger entries (double-entry)
            // 1) Credit: Stock units account (increase stock units)
            await _ledgerRepo.AddAsync(new LedgerEntry {
                EventId = ev.Id,
                EntryType = "STOCK_UNIT",
                Account = $"STOCK:{{ev.Symbol}}:USER:{{ev.UserId}}",
                Symbol = ev.Symbol,
                Quantity = ev.Quantity,
                DebitCredit = 'C'
            });

            // 2) Debit: Company cash outflow (cash decreased)
            await _ledgerRepo.AddAsync(new LedgerEntry {
                EventId = ev.Id,
                EntryType = "CASH_OUTFLOW",
                Account = "CASH:COMPANY:INR",
                AmountInr = companyOutflow,
                DebitCredit = 'D'
            });

            // 3) Fees entries
            if (brokerage > 0)
            {
                await _ledgerRepo.AddAsync(new LedgerEntry {
                    EventId = ev.Id,
                    EntryType = "FEE_BROKERAGE",
                    Account = "EXPENSE:BROKERAGE",
                    AmountInr = brokerage,
                    DebitCredit = 'D'
                });
            }
            if (stt > 0)
            {
                await _ledgerRepo.AddAsync(new LedgerEntry {
                    EventId = ev.Id,
                    EntryType = "FEE_STT",
                    Account = "EXPENSE:STT",
                    AmountInr = stt,
                    DebitCredit = 'D'
                });
            }
            if (gst > 0)
            {
                await _ledgerRepo.AddAsync(new LedgerEntry {
                    EventId = ev.Id,
                    EntryType = "FEE_GST",
                    Account = "EXPENSE:GST",
                    AmountInr = gst,
                    DebitCredit = 'D'
                });
            }

            // Persist reward event
            await _rewardRepo.AddAsync(ev);

            // Update holdings (company delivered full shares to user)
            var holding = new Holding
            {
                UserId = ev.UserId,
                Symbol = ev.Symbol,
                Quantity = ev.Quantity,
                // store average price paid per share (company cost per share)
                AvgPriceInr = price
            };
            await _holdingRepo.UpsertHoldingAsync(holding);

            return ev;
        }
    }
}
