# Bootstrap WebApi
Solution scafolding to quickly bootstrap an API.

## Global pattern
- [Hexagonal architecture](https://en.m.wikipedia.org/wiki/Hexagonal_architecture_(software))
- [Command and Query Segregation (CQS)](https://www.martinfowler.com/bliki/CommandQuerySeparation.html) using Mediator
- In memory [Event driven architecture](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/event-driven)

## Api
- [Url versioning](https://restfulapi.net/versioning)
- [Swagger](https://swagger.io/)
- [Problem details](https://problemdetails.com/)
- [Health check](https://microservices.io/patterns/observability/health-check-api.html)

## Infrastructure
- [EntityFramework](https://learn.microsoft.com/en-us/aspnet/entity-framework)
- TransactionScope
- Database migration with FluentMigrator

## Testability
- Unit tests with XUnit & FluentAssertions
- Acceptance tests using Specflow on top of api
- Integration by only annotating ```@Integration``` some of acceptance tests

## Telemetry [todo]
## Feature Management [todo]
