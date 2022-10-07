using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.Domain;
using Bootstrap.Domain.Customers;

namespace Bootstrap.Application.Customers;

public record RenameCustomerCommand(Guid Id, string FirstName, string LastName) : ICommand;

public class RenameCustomerCommandHandler : ICommandHandler<RenameCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public RenameCustomerCommandHandler(ICustomerRepository customerRepository) =>
        _customerRepository = customerRepository;

    public async Task Handle(RenameCustomerCommand command)
    {
        var customer = await _customerRepository.Get(command.Id);

        customer.Rename(command.FirstName, command.LastName);

        await _customerRepository.Save(customer);
    }
}
