using System.ComponentModel.DataAnnotations;
using Bootstrap.Application.Customers;
using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.BuildingBlocks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.WebApi.Controllers.V1;

[ApiController]
[Route("v{v:apiVersion}/customers")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class CustomerController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CustomerController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Register a new customer from firstname and lastname.
    /// </summary>
    /// <response code="201">Returns the newly created customer</response>
    /// <response code="400">If the item is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterNewCustomer([Required] NameBody body)
    {
        await _commandDispatcher.Dispatch(new RegisterNewCustomerCommand(body.FirstName, body.LastName));

        return this.CreatedAtAction(nameof(this.ListCustomers), new{});
    }

    [HttpPut("{id}/rename")]
    public async Task RenameCustomer([Required] NameBody body, [Required] [FromRoute] Guid id)
        => await _commandDispatcher.Dispatch(new RenameCustomerCommand(id, body.FirstName, body.LastName));

    [HttpGet]
    public async Task<IEnumerable<CustomerListItemDto>> ListCustomers()
        => await _queryDispatcher.Dispatch(new ListCustomersQuery());


    // Bodies

    public class NameBody
    {
        [Required] public string FirstName { get; set; } = default!;
        [Required] public string LastName { get; set; } = default!;
    }
}
