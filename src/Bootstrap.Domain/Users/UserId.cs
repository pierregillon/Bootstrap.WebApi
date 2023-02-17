namespace Bootstrap.Domain.Users;

public record UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
    public static UserId Rehydrate(Guid value) => new(value);

    public static implicit operator Guid(UserId userId) => userId.Value;
}
