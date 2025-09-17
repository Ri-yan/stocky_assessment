
# Edge cases & How this design handles them

## Duplicate reward events / replay attacks
- API accepts an optional `IdempotencyKey` in the /reward request. The in-memory `InMemoryIdempotencyService` attempts to register the key. If already registered, the service returns the existing event (simple behavior).
- In production: store idempotency keys in DB with expiry and tie to event/result to ensure idempotence across restarts.

## Stock splits, mergers, delisting
- The `RewardEvent` stores symbol and quantity at time of reward.
- Adjustments (splits/merger) should be applied as separate ledger adjustments:
  - Create an adjustment reward or ledger entry that scales existing holdings (e.g., multiply quantities by split ratio) and log a reference to the corporate action.
- Delisting: company should create a special ledger entry and flag holdings as non-tradable; valuation logic needs custom handling and communication to users.

## Rounding errors in INR valuation
- Use `NUMERIC(18,6)` for share quantities and `NUMERIC(18,4)` for INR amounts (this repo uses `decimal` with rounding where needed).
- Round monetary amounts at the last step when presenting to users, but keep internal calculations at full precision where possible.

## Price API downtime or stale data
- The `IPriceService` should implement caching, TTL, and circuit-breaker in production.
- On downtime, present user's last-known price and mark price as stale. Offer an "asOf" timestamp with valuations.

## Adjustments/refunds of previously given rewards
- Create a new event type (e.g., `ADJUSTMENT`) that references the original `RewardEvent.Id` and creates inverse ledger entries.
- Never delete events — always record reversing entries for auditability.

## Scaling notes
- Replace in-memory stores with a relational DB (Postgres / SQL Server).
- Use transactional outbox or two-phase commits for ledger operations to ensure ledger and holdings remain consistent.
- Use partitioned tables for ledger entries and indexes on event_id, user_id, symbol.
- Use distributed cache (Redis) for price cache; have a dedicated price ingestion microservice.
- Add read replica(s) / materialized views for reporting endpoints to avoid slowing down transactional paths.
