
# Stocky - In-Memory .NET Core Web API (Sample)

This repository implements a **Stocky** prototype: an in-memory .NET Core Web API that records stock rewards (shares given to users) and maintains a double-entry ledger for company cash outflow and stock unit bookkeeping.

This is intentionally implemented as a single-repo sample (no external DB) meant for local development, tests, and demonstration. It includes:

- REST endpoints:
  - `POST /reward`
  - `GET  /today-stocks/{userId}`
  - `GET  /historical-inr/{userId}`
  - `GET  /stats/{userId}`
  - `GET  /portfolio/{userId}` (bonus)

- In-memory persistence (thread-safe)
- A background price updater that simulates hourly stock prices
- Idempotency support for reward events (prevent replay duplicates)
- Double-entry ledger creation per reward event:
  - Credit: Stock units (by symbol, qty)
  - Debit: INR cash outflow (company cost)
  - Separate ledger entries for fees (brokerage, STT, GST)

## How to run (developer machine)

This is a .NET project; to run locally you need `.NET 7+` SDK installed.

1. Open a terminal at repository root.
2. Run: `dotnet restore`
3. Run: `dotnet run --project src/Stocky.Api`

API will start on the configured Kestrel address (see `Properties/launchSettings.json` or console output).

## Project structure

- `src/Stocky.Api` - the Web API (controllers, services, models)
- `docs/` - SQL schema and ER diagram notes
- `examples/` - sample HTTP requests

## Data / DB schema (relational design)

See `docs/schema.sql` for CREATE TABLE statements for RewardEvents, LedgerEntries, Holdings, Users (suggested).

## Notes about production-grade changes

This implementation is for demonstration. For production:
- Replace in-memory stores with a transactional relational DB (Postgres, SQL Server).
- Use proper idempotency keys stored in DB to avoid replay across restarts.
- Add authentication/authorization and rate limiting.
- Add robust error handling, monitoring, and data migrations.
- Use a real price source, caching, and circuit breakers for external API calls.

