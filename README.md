# DigitalWallet
A containerized digital wallet platform built with an event-driven architecture, using patterns like the Transactional Outbox, CDC (Change Data Capture), and CQRS.

## Tech Stack

| Layer | Technology |
|---|---|
| API | .NET |
| Projector | .NET Worker Service |
| Frameworks | ASP.NET Core, MassTransit, EF Core |
| Migrations | Dedicated migration runner |
| Primary DB | PostgreSQL |
| Document Store | MongoDB |
| CDC | Debezium |
| Message Broker | Redpanda |
| Gateway | NGINX |
| Container Orchestration | Docker Compose |

## Architecture Overview

### Module Structure

The solution is organized into self-contained modules, each owning its own PostgreSQL schema to isolate data boundaries at the database level. Snake_case naming conventions and the outbox configuration are handled centrally in `Shared.Infrastructure` via a `BaseDbContext`, so individual modules inherit consistent behavior without boilerplate.

### Authentication

The `Identity` module handles registration, login, JWT issuance, and refresh token rotation. Tokens are validated across all protected endpoints via ASP.NET Core middleware.

### Database

Database migrations are run as a **dedicated Docker Compose step**, separate from the API and worker services, avoiding race conditions that can occur when migrations run on app startup in multi-instance environments.

### Event Pipeline

The API writes domain changes to PostgreSQL transactionally, including an **outbox table** in the same transaction. **Debezium** monitors only the outbox via CDC and publishes events to **Redpanda**, guaranteeing at-least-once delivery. The **Projector** is a .NET Worker Service that consumes these events via **MassTransit** and updates the read models in **MongoDB**, decoupling the query side from the write model.

## Getting Started

The entire environment is containerized. Choose the command for your Operating System to clone, configure, and launch the system.

### 🐧 Linux / macOS

```bash
git clone https://github.com/viktorgyorgy/DigitalWallet.git && \
cd DigitalWallet && \
cp .env.example .env && \
docker compose up -d --build
```

### 🪟 Windows

```powershell
git clone https://github.com/viktorgyorgy/DigitalWallet.git; `
cd DigitalWallet; `
copy .env.example .env; `
docker compose up -d --build
```

## Accessing the Services

Once running, the following local URLs are available:

> **Note:** Tool dashboards are defined in `docker-compose.override.yml`, which Docker Compose picks up automatically in local development. Database ports are not exposed at any level. To run without the dashboards (e.g. in a CI or production-like environment), use `docker compose -f docker-compose.yml up -d` explicitly.

| URL | Description |
|---|---|
| `http://api.localhost` | REST API (redirects to Scalar API docs at `/scalar/v1`) |
| `http://tools.localhost` | Unified dashboard (Debezium UI, pgAdmin, Mongo Express, Redpanda Console) |
| `http://pgadmin.localhost` | pgAdmin — PostgreSQL management |
| `http://debezium.localhost` | Debezium UI — CDC connector management |
| `http://redpanda.localhost` | Redpanda Console — topic/message browser |
| `http://mongo.localhost` | Mongo Express — MongoDB management |

## Roadmap

- Wallets & Transactions — wallet creation, deposit, and withdrawal
- MassTransit error handling — poison pill handling and custom retry configuration
- OpenTelemetry — distributed tracing across the API and Projector
