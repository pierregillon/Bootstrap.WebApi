using Bootstrap.BuildingBlocks.Aggregates;
using Bootstrap.Domain.Users.Security;

namespace Bootstrap.Domain.Users;

public class User : EventDrivenAggregateRoot<UserId>
{
    private User(
        UserId id,
        EmailAddress emailAddress,
        PasswordHash passwordHashHash) : base(id)
    {
        EmailAddress = emailAddress;
        PasswordHash = passwordHashHash;
    }

    public EmailAddress EmailAddress { get; }
    public PasswordHash PasswordHash { get; }

    public static User Register(
        EmailAddress emailAddress,
        PasswordHash passwordHash
    )
    {
        var user = new User(
            UserId.New(),
            emailAddress,
            passwordHash
        );
        user.StoreEvent(new UserRegistered(user.Id, emailAddress, passwordHash));
        return user;
    }

    public static User Rehydrate(
        UserId id,
        EmailAddress emailAddress,
        PasswordHash passwordHash) =>
        new(id, emailAddress, passwordHash);
}
