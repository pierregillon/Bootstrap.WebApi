namespace Bootstrap.Domain.Users;

public interface IUserRepository
{
    Task Save(User user);
}
