# Bootstrap WebApi
**Solution scaffolding** to quickly bootstrap an API.

The idea is to provide a set of *ready-to-use* projects with code dependencies and examples.
It is especilly useful and time saving when you already know the api is not just a CRUD component but
but a **component that will grow fast with complex domain code**.

In DDD, the idea is to have 1 BC per service, but if you are challenging boundaries and not sure
yet, it is preferable to keep all your code base in the same service. You can easily adjust 
this project to work with multiple bounded context.


## Architecture
- [Hexagonal architecture](https://en.m.wikipedia.org/wiki/Hexagonal_architecture_(software)) is order to decouple domain code from infrastructure purpose
- [Command and Query Segregation (CQS)](https://www.martinfowler.com/bliki/CommandQuerySeparation.html) using [Mediator](https://github.com/jbogard/MediatR) to separate write code from read code, avoiding to mix up models
- In memory [Event driven architecture](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/event-driven), in order to transform procedural code to reactive programming by chaining side effects based on (domain) events

## Domain code
- [Domain Driven Design](https://www.geeksforgeeks.org/domain-driven-design-ddd/) to express domain with tactical patterns like aggregate, entities, value object, domain event, ...
- Repository pattern

## Api
- [Url versioning](https://restfulapi.net/versioning) to avoid breaking changes on api evolution
- [Swagger](https://swagger.io/) to document and explore which end points are available
- [Problem details](https://problemdetails.com/) to normalize error management
- [Health check](https://microservices.io/patterns/observability/health-check-api.html) to provide routes exposing application state (/hc, /liveness)

## Infrastructure
- [Entity framework](https://learn.microsoft.com/en-us/aspnet/entity-framework) to simplify database read and write
- [TransactionScope](https://www.codeproject.com/articles/690136/all-about-transactionscope) to handle domain transaction by code, not by database
- Database migration with [FluentMigrator](https://fluentmigrator.github.io/) in order to migrate the database easily with fluent code

## Testability
- Unit tests with [xUnit](https://xunit.net/) & [FluentAssertions](https://fluentassertions.com/)
```csharp
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Renaming_a_customer_requires_a_non_empty_first_name(string emptyFirstName)
    {
        Customer customer = A.Customer;

        var action = () => customer.Rename(emptyFirstName, "some last name");

        action
            .Should()
            .Throw<ArgumentException>();
    }
``` 
- Acceptance tests using [Specflow](https://specflow.org/) on top of api, in order to have human readable specifications
```gerkin
Scenario: Registering a new customer lists it
	When I register the following new customer
		| First name | Last name |
		| John       | Doe       |
	Then the customer list is
		| Full name |
		| John Doe  |
```
- Integration by only annotating ```@Integration``` some of acceptance tests
```gerkin
@Integration
Scenario: Renaming a customer has his name well updated in the list
	When I rename the customer "John Doe" to "Albert Rose"
	Then the "John Doe" customer is now listed with full name "Albert Rose"
```
## Image build and deployment
- Containerization with [Dockerfile](https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows) [todo]
- Basic github action to trigger CI [dotnet.yml](.github/workflows/dotnet.yml)

## Observability
- [Open telemetry](https://opentelemetry.io/docs/instrumentation/net/) [todo] to normalize logs, traces and metrics

## Feature flags
- [.net Feature management](https://learn.microsoft.com/en-us/azure/azure-app-configuration/use-feature-flags-dotnet-core?tabs=core5x) in order to easily and dynamically activate or deactivate features from configuration.
