
-- Suggested relational schema (Postgres / SQL Server compatible)

CREATE TABLE users (
    id UUID PRIMARY KEY,
    name TEXT,
    email TEXT
);

CREATE TABLE reward_events (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id),
    symbol VARCHAR(32) NOT NULL,
    quantity NUMERIC(18,6) NOT NULL,
    reward_timestamp TIMESTAMP WITH TIME ZONE NOT NULL,
    idempotency_key VARCHAR(128), -- for dedupe/replay protection
    metadata JSONB,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT now()
);

CREATE TABLE ledger_entries (
    id UUID PRIMARY KEY,
    event_id UUID NULL REFERENCES reward_events(id),
    entry_type VARCHAR(32) NOT NULL, -- STOCK_UNIT | CASH_OUTFLOW | FEE
    account VARCHAR(128) NOT NULL,
    symbol VARCHAR(32), -- when entry relates to stock units
    quantity NUMERIC(18,6), -- for stock unit entries
    amount_inr NUMERIC(18,4), -- for cash/fee entries
    debit_credit CHAR(1) NOT NULL, -- 'D' or 'C'
    created_at TIMESTAMP WITH TIME ZONE DEFAULT now()
);

CREATE TABLE holdings (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id),
    symbol VARCHAR(32) NOT NULL,
    quantity NUMERIC(18,6) NOT NULL,
    avg_price_inr NUMERIC(18,4) NULL,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT now(),
    UNIQUE(user_id, symbol)
);

-- Indexes and foreign keys omitted for brevity
