using System.ComponentModel.DataAnnotations;
using Bootstrap.Application;
using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.BuildingBlocks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.WebApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICommandDispatcher commandDispatcher;
    private readonly IQueryDispatcher queryDispatcher;

    public CustomerController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        this.commandDispatcher = commandDispatcher;
        this.queryDispatcher = queryDispatcher;
    }

    [HttpPost]
    public async Task Post([Required] RegisterNewCustomerBody body) 
        => await this.commandDispatcher.Dispatch(new RegisterNewCustomerCommand(body.FirstName, body.LastName));

    [HttpPut("{id}/rename")]
    public async Task Put([Required] RenameCustomerBody body, [Required] [FromRoute] Guid id)
        => await this.commandDispatcher.Dispatch(new RenameCustomerCommand(id, body.FirstName, body.LastName));

    [HttpGet]
    public async Task<IEnumerable<CustomerListItemDto>> Get() 
        => await this.queryDispatcher.Dispatch(new ListCustomers());


    public record RenameCustomerBody(string FirstName, string LastName);
}
