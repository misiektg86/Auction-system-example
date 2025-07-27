# 🛒 Auction Buy Now System

> **A showcase project** demonstrating high-performance, modern software architecture in .NET. Built to present **concurrency handling**, **Clean Architecture**, **CQRS**, **Redis**, **SQL Server**, and future refactorings toward **DDD**, **Event Sourcing**, and **streaming-first architectures (Kappa/Lambda)**.
---

## 🎯 Project Goals

- ✅ Handle **high-concurrency "Buy Now"** operations with first-come, first-served guarantees
- ✅ Showcase **Clean Architecture** principles (separation of concerns, testability)
- 🚧 Refactor toward **DDD (Domain-Driven Design)** structure
- 🚧 Introduce **Event Sourcing** using existing SQL as event store
- 🚧 Build **CQRS** separation: command model (write) + projection model (read)
- 🚧 Add **reactive frontend** (Blazor or React + SignalR)
- 🚧 Integrate **messaging with Kafka / RabbitMQ**
- 🚧 Explore **Kappa** and **Lambda** architectures for real-time auction analytics

---

## 🧱 Current Architecture

```
┌────────────────────────────┐
│        API (Web)           │ ← ASP.NET Core Controllers / Minimal API
├────────────────────────────┤
│  Application Layer (CQRS)  │ ← Command/Query Handlers, Interfaces
├────────────────────────────┤
│       Domain Model         │ ← Entities, Value Objects
├────────────────────────────┤
│     Infrastructure Layer   │ ← SQL Server, Redis, Repositories, Services
└────────────────────────────┘
```

---

## 💥 High-Concurrency Strategy

- Redis stores stock count: `stock:{itemId}`  
- Atomic `DECR` ensures only available stock is reserved
- If Redis < 0 → return "sold out"
- If OK → SQL Server updates reservation (`pessimistic concurrency`)
- SQL failure → rollback Redis via `INCR`
- 🔐 Thread-safe and fast — no locks, no race conditions

---

## 🧪 Testing Strategy

### ✅ Unit Tests
- Pure logic (e.g. `BuyNowHandler`) tested with mocks
- Fast and isolated

### ✅ Integration Tests
- Run Redis + SQL in Docker via Testcontainers
- Seed data, perform real API calls
- Setup + teardown included

---

## 🧰 Technology Stack

| Layer          | Technology                      |
|----------------|----------------------------------|
| Backend        | ASP.NET Core 9                   |
| Database       | SQL Server via Docker            |
| Cache / Lock   | Redis                            |
| Tests          | xUnit, Testcontainers            |
| Messaging      | (Planned: Kafka, RabbitMQ)       |
| Architecture   | Clean Architecture (→ DDD, CQRS) |
| Frontend       | (Planned: React or Blazor)       |
| Dev Env        | VS Code / Docker / macOS         |

---

## 📁 Folder Structure

```
auction-buy-now/
├── src/
│   ├── AuctionSystem.Api/             ← ASP.NET Core API
│   ├── AuctionSystem.Application/     ← CQRS Handlers, Interfaces
│   ├── AuctionSystem.Domain/          ← Domain models (to refactor to DDD)
│   └── AuctionSystem.Infrastructure/  ← Redis, SQL, Repositories
├── tests/
│   ├── AuctionSystem.UnitTests/       ← Unit tests
│   └── AuctionSystem.IntegrationTests/← Testcontainers: SQL + Redis
├── docker-compose.yml                 ← SQL + Redis containers
└── README.md
```

---

## 🔮 Roadmap

### ✅ Phase 1: (Current)
- [x] ASP.NET Core API
- [x] Redis stock locking
- [x] SQL Server reservation
- [x] Integration + unit tests

### 🚧 Phase 2: DDD + CQRS
- [ ] Restructure to aggregates, VOs, domain events
- [ ] Add read model with projections (EF Core or NoSQL)
- [ ] Sync read model via background processor or events

### 🚧 Phase 3: Event Sourcing
- [ ] Store events in SQL table
- [ ] Replay event stream to rebuild item state
- [ ] Snapshot optimization

### 🚧 Phase 4: Messaging + Streaming
- [ ] Use Kafka or RabbitMQ for `ItemBought`, `OutOfStock` etc.
- [ ] Reactive UI (Blazor/React + SignalR)
- [ ] Real-time analytics stream: Kafka → Stream Processing → Dashboard

### 🚧 Phase 5: Architectures
- [ ] Kappa architecture stream → materialized views (1 system)
- [ ] Lambda architecture: batch + streaming + serving layer

---

## 🚀 Running the Project

```bash
# Start SQL + Redis
docker-compose up -d

# Run the API
dotnet run --project src/AuctionSystem.Api

# Run all tests
dotnet test tests/AuctionSystem.UnitTests
dotnet test tests/AuctionSystem.IntegrationTests
```

---

## 👤 Author

**Michał Kocik**  
[GitHub](https://github.com/misiektg86) | [LinkedIn](https://www.linkedin.com/in/michal-kocik-6102189a/)

This project is intended as a **portfolio-quality example** for software architecture, concurrency, and distributed system patterns.
