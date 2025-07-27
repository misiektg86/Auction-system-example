# 🛒 Auction Buy Now System

> High-throughput auction system with **"Buy Now"** capability, implemented in **.NET 9** using **Clean Architecture** and **Microsoft technologies** (SQL Server, Redis, ASP.NET Core). Designed to handle massive concurrent traffic with `first come, first served` logic and concurrency safety.

---

## 🧱 Architecture

This project follows the **Clean Architecture / Onion Architecture** style with a clear separation of concerns:

```
┌────────────────────────────┐
│        API (Web)           │ ← ASP.NET Core Controllers / Minimal API
├────────────────────────────┤
│  Application Layer (CQRS)  │ ← Command/Query Handlers, Interfaces
├────────────────────────────┤
│       Domain Model         │ ← Entities, Value Objects
├────────────────────────────┤
│     Infrastructure Layer   │ ← SQL Server, Redis, Repository, Services
└────────────────────────────┘
```

---

## 🧪 Testing

This solution includes full test coverage:

### ✅ Unit Tests
- Tests for core application logic (`BuyNowHandler`)
- Mocks Redis and SQL access

### ✅ Integration Tests
- Use **Testcontainers** to run:
  - **SQL Server**
  - **Redis**
- The test suite:
  - Seeds data into SQL and Redis
  - Executes HTTP requests against the running API
  - Tears down containers automatically after the run

---

## ⚙️ Technology Stack

| Layer          | Technology                    |
|----------------|-------------------------------|
| API            | ASP.NET Core 9 (Minimal API)  |
| Database       | SQL Server via Docker         |
| Cache / Lock   | Redis                         |
| Messaging      | – (planned: RabbitMQ/Kafka)   |
| DI + CQRS      | MediatR-style Command/Query   |
| Frontend       | React or Blazor (planned)     |
| Testing        | xUnit, Testcontainers         |
| Dev Env        | VS Code / Docker / macOS      |

---

## 🚀 Buy Now Logic

- Redis holds real-time stock via `DECR` (atomic decrement)
- If Redis count drops below 0 → return “sold out” immediately
- If OK → proceed to SQL to reserve the item (pessimistic concurrency)
- If SQL fails → rollback Redis with `INCR`
- ✅ Ensures **"first come, first served"** under high traffic

---

## ▶️ Getting Started

### 1. Start Redis and SQL containers (optional)

```bash
docker-compose up -d
```

> Integration tests also spin up containers automatically

### 2. Run the API

```bash
dotnet run --project src/AuctionSystem.Api
```

### 3. Run Tests

```bash
dotnet test tests/AuctionSystem.UnitTests
dotnet test tests/AuctionSystem.IntegrationTests
```

---

## 📁 Folder Structure

```
auction-buy-now/
├── src/
│   ├── AuctionSystem.Api/             ← ASP.NET Core API
│   ├── AuctionSystem.Application/     ← CQRS logic
│   ├── AuctionSystem.Domain/          ← Domain models
│   └── AuctionSystem.Infrastructure/  ← Redis, SQL, Repositories
├── tests/
│   ├── AuctionSystem.UnitTests/       ← Pure logic tests
│   └── AuctionSystem.IntegrationTests/← Docker-based end-to-end tests
├── docker-compose.yml                 ← Redis + SQL containers
└── README.md
```

---

## 📌 Roadmap

- 🔐 Authentication and authorization
- 🌐 Real frontend: React or Blazor
- 🔁 Retry and circuit-breaker policies
- 📈 Load & stress testing (NBomber, k6)
- 💾 Event sourcing (in next version)
- 📦 EF Core migrations

---

## 📄 License

MIT © [Michał Kocik](https://github.com/misiektg86)