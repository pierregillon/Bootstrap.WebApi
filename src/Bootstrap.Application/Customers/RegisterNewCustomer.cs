using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.Domain.Customers;

namespace Bootstrap.Application.Customers;

public record RegisterNewCustomerCommand(string FirstName, string LastName) : ICommand<Guid>;

internal class RegisterNewCustomerHandler : ICommandHandler<RegisterNewCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repository;

    public RegisterNewCustomerHandler(ICustomerRepository repository) => _repository = repository;

    public async Task<Guid> Handle(RegisterNewCustomerCommand command)
    {
        var customer = Customer.Register(command.FirstName, command.LastName);

        await _repository.Save(customer);

        return customer.Id;
    }
}
