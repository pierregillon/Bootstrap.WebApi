using System.ComponentModel.DataAnnotations;
using Bootstrap.Application.Customers;
using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.BuildingBlocks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.WebApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CustomerController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpPost]
    public async Task Post([Required] RegisterNewCustomerBody body)
        => await _commandDispatcher.Dispatch(new RegisterNewCustomerCommand(body.FirstName, body.LastName));

    [HttpPut("{id}/rename")]
    public async Task Put([Required] RenameCustomerBody body, [Required] [FromRoute] Guid id)
        => await _commandDispatcher.Dispatch(new RenameCustomerCommand(id, body.FirstName, body.LastName));

    [HttpGet]
    public async Task<IEnumerable<CustomerListItemDto>> Get()
        => await _queryDispatcher.Dispatch(new ListCustomersQuery());


    public record RenameCustomerBody(string FirstName, string LastName);
}
