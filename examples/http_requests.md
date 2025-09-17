
# Example requests

## Reward a user (POST /reward)
POST /reward
Content-Type: application/json
{
  "userId": "8f14e45f-ea9f-4c12-9f4a-0a1c6b7d3e6f",
  "symbol": "RELIANCE",
  "quantity": 2.5,
  "timestamp": "2025-09-10T10:00:00Z",
  "idempotencyKey": "reward-20250910-8f14e45f"
}

## Get today's stocks
GET /today-stocks/8f14e45f-ea9f-4c12-9f4a-0a1c6b7d3e6f

## Get historical INR
GET /historical-inr/8f14e45f-ea9f-4c12-9f4a-0a1c6b7d3e6f

## Get stats
GET /stats/8f14e45f-ea9f-4c12-9f4a-0a1c6b7d3e6f

## Get portfolio
GET /portfolio/8f14e45f-ea9f-4c12-9f4a-0a1c6b7d3e6f
