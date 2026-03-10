# DigitalWallet

A containerized digital wallet platform built with an event-driven architecture, using patterns like the Transactional Outbox, CDC (Change Data Capture), and CQRS.

## Tech Stack

| Layer | Technology |
|---|---|
| API | .NET (ASP.NET Core) |
| Projector | .NET Worker Service |
| Migrations | EF Core + dedicated migration runner |
| Primary DB | PostgreSQL |
| Document Store | MongoDB |
| CDC | Debezium |
| Message Broker | Redpanda |
| Gateway | NGINX |
| Container Orchestration | Docker Compose |

## Architecture Overview

### Module Structure

Each module (e.g. `Users`) is split into isolated projects following a strict layered architecture:

| Project | Responsibility |
|---|---|
| `Domain` | Pure domain objects and logic — no framework dependencies |
| `Contracts` | Event definitions shared across modules |
| `Application` | Use cases and orchestration — no infrastructure concerns |
| `Infrastructure` | EF Core, repositories, outbox integration |
| `Api` | ASP.NET Core endpoints — no business logic |

### Database

Each module owns its own **PostgreSQL schema**, isolating data boundaries at the database level. Snake_case naming conventions and the outbox configuration are handled centrally in `Shared.Infrastructure` via a `BaseDbContext`, so individual modules inherit consistent behavior without boilerplate.

Database migrations are run as a **dedicated Docker Compose step**, separate from the API and worker services, avoiding race conditions that can occur when migrations run on app startup in multi-instance environments.

### Event Pipeline

The API writes domain changes to PostgreSQL transactionally, including an **outbox table** in the same transaction. **Debezium** monitors only the outbox via CDC and publishes events to **Redpanda**, guaranteeing at-least-once delivery. A **.NET Worker Service (Projector)** consumes these events and updates the read models in **MongoDB**, decoupling the query side from the write model.


## Getting started

The entire environment is containerized. Choose the command for your Operating System to clone, configure, and launch the system.

### 🐧 Linux / macOS

```bash
git clone https://github.com/viktorgyorgy/DigitalWallet.git && \
cd DigitalWallet && \
cp .env.example .env && \
docker compose up -d --build
```

### 🪟 Windows

```ps
git clone https://github.com/viktorgyorgy/DigitalWallet.git; `
cd DigitalWallet; `
copy .env.example .env; `
docker compose up -d --build
```


## Accessing the Services

Once running, the following local URLs are available:

> **Note:** Tool dashboards and database ports are not exposed in the base `docker-compose.yml`. They are defined in `docker-compose.override.yml`, which Docker Compose picks up automatically in local development. To run without them (e.g. in a CI or production-like environment), use `docker compose -f docker-compose.yml up -d` explicitly.

| URL | Description |
|---|---|
| `http://api.localhost` | REST API (redirects to Scalar API docs at `/scalar/v1`) |
| `http://tools.localhost` | Unified dashboard (Debezium UI, pgAdmin, Mongo Express, Redpanda Console) |
| `http://pgadmin.localhost` | pgAdmin — PostgreSQL management |
| `http://debezium.localhost` | Debezium UI — CDC connector management |
| `http://redpanda.localhost` | Redpanda Console — topic/message browser |
| `http://mongo.localhost` | Mongo Express — MongoDB management |



## Roadmap

### Phase 1 — Identity & Authentication *(up next)*
- Identity module with user credential management
- JWT-based authentication
- Refresh token support

### Phase 2 — Wallets & Transactions
- Wallet creation and management per user
- Deposit, withdrawal, and transfer operations

### Phase 3 — Resilience Patterns (Polly)
- Circuit breaker on downstream service calls
- Exponential backoff with jitter for retries
