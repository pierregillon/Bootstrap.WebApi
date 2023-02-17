using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.Domain.Users;
using Bootstrap.Domain.Users.Security;

namespace Bootstrap.Application.Users;

public record RegisterUserCommand(EmailAddress EmailAddress, Password Password) : ICommand<UserId>;

internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserId>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserFinder _userFinder;
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUserFinder userFinder,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _userFinder = userFinder;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserId> Handle(RegisterUserCommand command)
    {
        await CheckEmailAddressAndPhoneNumberAreNotAlreadyUsed(command.EmailAddress);

        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = User.Register(command.EmailAddress, passwordHash);

        await _userRepository.Save(user);

        return user.Id;
    }

    private async Task CheckEmailAddressAndPhoneNumberAreNotAlreadyUsed(EmailAddress emailAddress)
    {
        if (await _userFinder.AnyUserAlreadyRegisteredWith(emailAddress))
        {
            throw new EmailAddressAlreadyUsedByAnotherUserException();
        }
    }
}
