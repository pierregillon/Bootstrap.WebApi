# Bootstrap WebApi
Solution scafolding to quickly bootstrap an API.

## Global pattern
- Hexagonal architecture
- Command and Query Segregation (CQS) using Mediator
- Event driven architecture

## Api
- Url versioning
- Swagger
- Problem details
- Health check

## Infrastructure
- EntityFramework
- TransactionScope
- Database migration with FluentMigrator

## Testability
- Unit tests with XUnit & FluentAssertions
- Acceptance tests using Specflow on top of api
- Integration by only annotating ```@Integration``` some of acceptance tests

## Telemetry [todo]
## Feature Management [todo]