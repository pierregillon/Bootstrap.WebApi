using Bootstrap.BuildingBlocks.DomainEvents;
using Bootstrap.Domain.Users.Security;

namespace Bootstrap.Domain.Users;

public record UserRegistered(
    UserId UserId,
    EmailAddress EmailAddress,
    PasswordHash PasswordHash
) : IDomainEvent;
