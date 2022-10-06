using Bootstrap.BuildingBlocks;
using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.Domain;

namespace Bootstrap.Application;

public record RegisterNewCustomer(string FirstName, string LastName) : ICommand;

internal class RegisterNewCustomerHandler : ICommandHandler<RegisterNewCustomer>
{
    private readonly ICustomerRepository _repository;

    public RegisterNewCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    async Task ICommandHandler<RegisterNewCustomer>.Handle(RegisterNewCustomer command)
    {
        var customer = Customer.Register(command.FirstName, command.LastName);

        await _repository.Save(customer);
    }
}