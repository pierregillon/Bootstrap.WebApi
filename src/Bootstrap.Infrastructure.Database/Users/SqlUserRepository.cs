using Bootstrap.BuildingBlocks.DomainEvents;
using Bootstrap.Domain.Users;

namespace Bootstrap.Infrastructure.Database.Users;

internal class SqlUserRepository : IUserRepository
{
    private readonly BootstrapDbContext _dbContext;
    private readonly IDomainEventPublisher _domainEventPublisher;

    public SqlUserRepository(BootstrapDbContext dbContext, IDomainEventPublisher domainEventPublisher)
    {
        _dbContext = dbContext;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task Save(User user)
    {
        var entity = await _dbContext.Users.FindAsync((Guid)user.Id);
        if (entity is not null)
        {
            throw new NotImplementedException("Cannot update an existing user");
        }

        await _dbContext.Users.AddAsync(new UserEntity
        {
            Id = Guid.NewGuid(), EmailAddress = user.EmailAddress, Password = user.PasswordHash
        });

        await _domainEventPublisher.Publish(user.UncommittedEvents);
    }
}
