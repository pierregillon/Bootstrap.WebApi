using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.Domain;

namespace Bootstrap.Application;

public record RenameCustomerCommand(Guid Id, string FirstName, string LastName) : ICommand;

public class RenameCustomerCommandHandler : ICommandHandler<RenameCustomerCommand>
{
    private readonly ICustomerRepository customerRepository;

    public RenameCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        this.customerRepository = customerRepository;
    }

    async Task ICommandHandler<RenameCustomerCommand>.Handle(RenameCustomerCommand command)
    {
        var customer = await this.customerRepository.Get(command.Id);

        customer.Rename(command.FirstName, command.LastName);

        await this.customerRepository.Save(customer);
    }
}