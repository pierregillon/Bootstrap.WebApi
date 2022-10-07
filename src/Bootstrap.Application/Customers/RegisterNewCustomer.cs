using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.Domain;
using Bootstrap.Domain.Customers;

namespace Bootstrap.Application.Customers;

public record RegisterNewCustomerCommand(string FirstName, string LastName) : ICommand;

internal class RegisterNewCustomerHandler : ICommandHandler<RegisterNewCustomerCommand>
{
    private readonly ICustomerRepository _repository;

    public RegisterNewCustomerHandler(ICustomerRepository repository) => _repository = repository;

    public async Task Handle(RegisterNewCustomerCommand command)
    {
        var customer = Customer.Register(command.FirstName, command.LastName);

        await _repository.Save(customer);
    }
}
